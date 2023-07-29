using cfeya_webapp.Data;
using cfeya_webapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;

namespace cfeya_webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _iweb;

        public HomeController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IWebHostEnvironment iweb)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _iweb = iweb;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Fairy")]
        public IActionResult Fairy()
        {
            return View();
        }

        [Authorize(Roles = "Cinderella")]
        public IActionResult Cinderella()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactForm contact)
        {
            var msg = "";
            try
            {
                string htmlFilePath = Path.Combine(_iweb.WebRootPath, "EmailTemplate", "ContactFormTemplate.html");
                string Body = System.IO.File.ReadAllText(htmlFilePath);
                Body = Body.Replace("[firstname]", contact.Firstname);
                Body = Body.Replace("[lastname]", contact.Lastname);
                Body = Body.Replace("[email]", contact.Email);
                Body = Body.Replace("[message]", contact.Message);
                await _emailSender.SendEmailAsync("dreamcomestrue@cfeya.com", "You got new Contact Form", Body);
                msg = "Thanks! your message sent Successfully! We will get back to you soon.";
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            TempData["msg"] = msg;
            return RedirectToAction("ContactUs");
        }


        public IActionResult Stories()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //Method for Saving wishes and stories 
        [HttpPost]
        public string Save_Stories_Wishes(string story, string wish, int id)
        {
            var story_wish_Obj = _context.TblWish.Find(id);
            story_wish_Obj!.StoryText = story;
            story_wish_Obj!.WishText = wish;
            story_wish_Obj!.SubmitedDate = DateTime.Now;
            story_wish_Obj!.Status = "Floating in the Wish Clouds";
            story_wish_Obj!.LastModifiedDate = DateTime.Now;
            //var story_wish_Obj = new Wishes
            //{
            //    StoryText = story,
            //    WishText = wish,
            //    CreatedDate = DateTime.Now,
            //    SubmitedDate = DateTime.Now,
            //    Status = "Floating in the Wish Clouds",
            //    CreatedBy = _userManager.GetUserAsync(User).Result.UserName
            //};
            //_context.TblWish.Add(story_wish_Obj);
            _context.Entry(story_wish_Obj).State = EntityState.Modified;
            _context.SaveChanges();
            return "Success";
        }

        //For just saving story to database
        [HttpPost]
        public int SaveStory(string story, string wish, int? id)
        {
            if (id == null)
            {
                var story_wish_Obj = new Wishes
                {
                    StoryText = story,
                    WishText = wish,
                    CreatedDate = DateTime.Now,
                    Status = "Pending",
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = _userManager.GetUserAsync(User).Result.UserName,
                    CreatedByUserId = _userManager.GetUserAsync(User).Result.Id
                };
                _context.TblWish.Add(story_wish_Obj);
                _context.SaveChanges();
                return story_wish_Obj.Id;
            }
            else
            {
                var story_wish_Obj = _context.TblWish.Find(id);
                story_wish_Obj!.StoryText = story;
                story_wish_Obj!.LastModifiedDate = DateTime.Now;
                _context.Entry(story_wish_Obj).State = EntityState.Modified;
                _context.SaveChanges();
                return story_wish_Obj.Id;
            }
        }

        //For just saving wish to database
        [HttpPost]
        public int SaveWish(string story, string wish, int? id)
        {
            if (id == null)
            {
                var story_wish_Obj = new Wishes
                {
                    StoryText = story,
                    WishText = wish,
                    CreatedDate = DateTime.Now,
                    Status = "Pending",
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = _userManager.GetUserAsync(User).Result.UserName,
                    CreatedByUserId = _userManager.GetUserAsync(User).Result.Id
                };
                _context.TblWish.Add(story_wish_Obj);
                _context.SaveChanges();
                return story_wish_Obj.Id;
            }
            else
            {
                var story_wish_Obj = _context.TblWish.Find(id);
                story_wish_Obj!.WishText = wish;
                story_wish_Obj!.LastModifiedDate = DateTime.Now;
                _context.Entry(story_wish_Obj).State = EntityState.Modified;
                _context.SaveChanges();
                return story_wish_Obj.Id;
            }
        }


        //For Changing Role of Current User
        [HttpPost]
        public async Task<string> ChangeRoleAsync(string Current_role, string new_Role)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            await _userManager.AddToRoleAsync(user, new_Role);
            await _userManager.RemoveFromRoleAsync(user, Current_role);
            await _signInManager.RefreshSignInAsync(user);
            if (new_Role == "Fairy")
            {
                user.How_Many_Times_Fairy += 1;
                user.Role = new_Role;
                await _userManager.UpdateAsync(user);
            }
            if (new_Role == "Cinderella")
            {
                user.How_Many_Times_Cinderella += 1;
                user.Role = new_Role;
                await _userManager.UpdateAsync(user);
            }
            return "Success";
        }


        //For checking if user profile is completed or not
        public bool CheckUserProfileStatus(string name)
        {
            var user = _userManager.FindByNameAsync(name).Result;
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
    }
}