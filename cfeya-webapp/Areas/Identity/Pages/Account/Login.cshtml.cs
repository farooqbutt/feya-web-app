// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using cfeya_webapp.Models;
using System.Net.Mail;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace cfeya_webapp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Username / Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        //Function to check if entered data is a valid email id or not.
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var username_email = Input.Email;
                if (IsValidEmail(username_email))
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    var signInResult = new SignInResult();
                    if (user != null)
                    {
                        signInResult = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password,
                            Input.RememberMe, lockoutOnFailure: false);
                    }
                    if (signInResult.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        if (user.Is18 == false)
                        {
                            await _signInManager.SignOutAsync();
                            ModelState.AddModelError(string.Empty, "Your Account is on Hold due to Under 18 age policy!");
                            return Page();
                        }
                        if (CheckUserProfileStatus(Input.Email))
                        {
                            if (UserRoleReturner(Input.Email) == "Fairy")
                            {
                                return LocalRedirect("/Home/Fairy");
                            }
                            else if (UserRoleReturner(Input.Email) == "Cinderella")
                            {
                                return LocalRedirect("/Home/Cinderella");
                            }
                            else
                            {
                                return LocalRedirect(returnUrl);
                            }
                        }
                        else
                        {
                            TempData["Notification"] = "Thanks for Registering with us! Please Complete your Profile First.";
                            return LocalRedirect("/Identity/Account/Manage");
                        }
                    }
                    else if (signInResult.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        if (user == null)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                        else if (user.EmailConfirmed == false)
                        {
                            ModelState.AddModelError(string.Empty, "Confirm your email first!");
                            return Page();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }
                }
                else
                {
                    var username = Input.Email;
                    var user = await _userManager.FindByNameAsync(username);
                    var signInResult = await _signInManager.PasswordSignInAsync(username, Input.Password,
                        Input.RememberMe, lockoutOnFailure: false);
                    if (signInResult.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        if (user.Is18 == false)
                        {
                            await _signInManager.SignOutAsync();
                            ModelState.AddModelError(string.Empty, "Your Account is on Hold due to Under 18 age policy!");
                            return Page();
                        }
                        if (CheckUserProfileStatus(Input.Email))
                        {
                            if (UserRoleReturner(Input.Email) == "Fairy")
                            {
                                return LocalRedirect("/Home/Fairy");
                            }
                            else if (UserRoleReturner(Input.Email) == "Cinderella")
                            {
                                return LocalRedirect("/Home/Cinderella");
                            }
                            else
                            {
                                return LocalRedirect(returnUrl);
                            }
                        }
                        else
                        {
                            TempData["Notification"] = "Thanks for Registering with us! Please Complete your Profile First.";
                            return LocalRedirect("/Identity/Account/Manage");
                        }
                    }
                    else if (signInResult.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        if (user == null)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                        else if (user.EmailConfirmed == false)
                        {
                            ModelState.AddModelError(string.Empty, "Confirm your email first!");
                            return Page();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        public bool CheckUserProfileStatus(string email)
        {
            var user = new ApplicationUser();
            if (IsValidEmail(email))
            {
                user = _userManager.FindByEmailAsync(email).Result;
            }
            else
            {
                user = _userManager.FindByNameAsync(email).Result;
            }
            string firstname = user.Firstname;
            string lastname = user.Lastname;
            DateTime? dob = user.DOB;
            string gender = user.Gender;
            string cellphone = user.CellPhone;
            string street = user.Street;
            string city = user.City;
            string state = user.State;
            string zip = user.Zip;
            if (firstname == null || firstname == "" || lastname == null || lastname == "" || dob == null
                || gender == null || gender == "" ||
                cellphone == null || cellphone == "" || street == null || street == "" ||
                city == null || city == "" || state == null || state == "" || zip == null || zip == "") 
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string UserRoleReturner(string username)
        {
            if (IsValidEmail(username))
            {
                var user = _userManager.FindByEmailAsync(username).Result;
                return user.Role;
            }
            else
            {
                var user = _userManager.FindByNameAsync(username).Result;
                return user.Role;
            }
        }
    }
}
