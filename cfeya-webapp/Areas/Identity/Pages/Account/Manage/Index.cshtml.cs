// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using cfeya_webapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace cfeya_webapp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string Firstname { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string Lastname { get; set; }
            //[Required]
            [Display(Name = "Middle Name")]
            public string Middlename { get; set; }
            [EmailAddress]
            [Required]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Date)]
            [Display(Name ="Date of Birth")]
            public DateTime? DOB { get; set; }
            [Required]
            public string Gender { get; set; }
            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name ="Cell Phone")]
            public string Cellphone { get; set; }
            //[Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name ="Home Phone")]
            public string Homephone { get; set; }
            //[Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name ="Work Phone")]
            public string Workphone { get; set; }
            [Required]
            public string Street { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            public string Zip { get; set; }

            [Display(Name = "Profile Picture")]
            public string ProfilePicture { get; set; }
            //[Phone]
            //[Display(Name = "Phone number")]
            //public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.Firstname;
            var lastName = user.Lastname;
            var middleName = user.Middlename;
            var email = user.Email;
            var city = user.City;
            var state = user.State;
            var zip = user.Zip;
            var street = user.Street;
            var dob = user.DOB;
            var gender = user.Gender;
            var cellphone = user.CellPhone;
            var homephone = user.HomePhone;
            var workphone = user.WorkPhone;
            var profilePicture = user.ProfilePicture;


            //Username = userName;

            Input = new InputModel
            {
                Firstname = firstName,
                Lastname = lastName,
                Middlename = middleName,
                Email = email,
                City = city,
                State = state,
                Zip = zip,
                Street = street,
                DOB = dob,
                Gender = gender,
                Cellphone = cellphone,
                Homephone = homephone,
                Workphone = workphone,
                ProfilePicture = profilePicture,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}

            var firtname = user.Firstname;
            var lastname = user.Lastname;
            var middlename = user.Middlename;
            var email = user.Email;
            var dob = user.DOB;
            var gender = user.Gender;
            var cellphone = user.CellPhone;
            var homephone = user.HomePhone;
            var workphone = user.WorkPhone;
            var street = user.Street;
            var city = user.City;
            var state = user.State;
            var zip = user.Zip;
            var profilePic = user.ProfilePicture;

            if (Input.Firstname != firtname)
            {
                user.Firstname = Input.Firstname;
                var setFirstname = await _userManager.UpdateAsync(user);
                if (!setFirstname.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set First Name.";
                    return RedirectToPage();
                }
            }
            if (Input.Lastname != lastname)
            {
                user.Lastname = Input.Lastname;
                var setLastname = await _userManager.UpdateAsync(user);
                if (!setLastname.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Last Name.";
                    return RedirectToPage();
                }
            }
            if (Input.Middlename != middlename)
            {
                user.Middlename = Input.Middlename;
                var setMiddlename = await _userManager.UpdateAsync(user);
                if (!setMiddlename.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Middle Name.";
                    return RedirectToPage();
                }
            }
            if (Input.DOB != dob)
            {
                user.DOB = Input.DOB;
                var setDOB = await _userManager.UpdateAsync(user);
                if (!setDOB.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Date of Birth.";
                    return RedirectToPage();
                }
            }
            if (Input.Gender != gender)
            {
                user.Gender = Input.Gender;
                var setGender = await _userManager.UpdateAsync(user);
                if (!setGender.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Gender.";
                    return RedirectToPage();
                }
            }
            if (Input.Cellphone != cellphone)
            {
                user.CellPhone = Input.Cellphone;
                var setCellphone = await _userManager.UpdateAsync(user);
                if (!setCellphone.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Cell Phone.";
                    return RedirectToPage();
                }
            }
            if (Input.Homephone != homephone)
            {
                user.HomePhone = Input.Homephone;
                var setHomephone = await _userManager.UpdateAsync(user);
                if (!setHomephone.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone Home Phone.";
                    return RedirectToPage();
                }
            }
            if (Input.Workphone != workphone)
            {
                user.WorkPhone = Input.Workphone;
                var setWorkphone = await _userManager.UpdateAsync(user);
                if (!setWorkphone.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Work Phone.";
                    return RedirectToPage();
                }
            }
            if (Input.Street != street)
            {
                user.Street = Input.Street;
                var setStreet = await _userManager.UpdateAsync(user);
                if (!setStreet.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Street.";
                    return RedirectToPage();
                }
            }
            if (Input.City != city)
            {
                user.City = Input.City;
                var setCity = await _userManager.UpdateAsync(user);
                if (!setCity.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set City.";
                    return RedirectToPage();
                }
            }
            if (Input.State != state)
            {
                user.State = Input.State;
                var setState = await _userManager.UpdateAsync(user);
                if (!setState.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set State.";
                    return RedirectToPage();
                }
            }
            if (Input.Zip != zip)
            {
                user.Zip = Input.Zip;
                var setZip = await _userManager.UpdateAsync(user);
                if (!setZip.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Zip.";
                    return RedirectToPage();
                }
            }

            if (Input.ProfilePicture!=profilePic)
            {
                //IFormFile file = Request.Form.Files.FirstOrDefault();
                //using (var dataStream = new MemoryStream())
                //{
                //    await file.CopyToAsync(dataStream);
                //    user.ProfilePicture = dataStream.ToArray();
                //}
                //await _userManager.UpdateAsync(user);
                user.ProfilePicture = Input.ProfilePicture;
                var setProfile = await _userManager.UpdateAsync(user);
                if (!setProfile.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Zip.";
                    return RedirectToPage();
                }
            }


            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"]  = "Your profile has been updated";
            if (user.Role == "Fairy")
            {
                return LocalRedirect("/Home/Fairy");
            }
            else
            {
                return LocalRedirect("/Home/Cinderella");
            }
        }
    }
}
