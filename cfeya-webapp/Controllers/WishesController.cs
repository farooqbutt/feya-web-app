using cfeya_webapp.Data;
using cfeya_webapp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace cfeya_webapp.Controllers
{
    public class WishesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Function for getting wishes related to current logged in user.
        public List<Wishes> GetWishes()
        {
            var username = _userManager.GetUserAsync(User).Result.UserName;
            var wishes = new List<Wishes>();
            if (User.IsInRole("Cinderella"))
            {
                wishes = _context.TblWish.Where(a => a.CreatedBy == username)
                    .OrderByDescending(a => a.SubmitedDate).ToList();
            }
            else
            {
                wishes = _context.TblWish.Where(a => a.PickedByFairy == username)
                    .OrderByDescending(a => a.Picked_up_date).ToList();
            }
            return wishes;
        }

        //For making paginated list
        public WishPaginationModel<Wishes> PaginatedWishList(List<Wishes> wishes, int? pageNumber, int pageSize)
        {
            var paginatedList = WishPaginationModel<Wishes>.Create(wishes, pageNumber ?? 1, pageSize);
            return paginatedList;
        }

        [Authorize(Roles ="Cinderella")]
        public IActionResult Index()
        {
            ViewBag.PendingWishes = GetWishes().Where(a => a.Status == "Pending").Count();
            ViewBag.ActiveWishes = GetWishes().Where(a => a.Status != "Pending" && a.Status != "Received").Count();
            ViewBag.FulfilledWishes = GetWishes().Where(a => a.Status == "Received").Count();
            return View();
        }

        //For getting all wishes for Fairy Role User
        [Authorize(Roles ="Fairy")]
        [HttpGet]
        public IActionResult AllWishes(string status, int? pageNumber, int? pagesize)
        {
            int pageSize = (int)(pagesize == null ? 30 : pagesize);
            if (status == null) 
            {
                var allwishes = GetWishes().Where(a => a.Status != "Pending").ToList();
                return View(PaginatedWishList(allwishes, pageNumber, pageSize));
            }
            else
            {
                var allwishes = GetWishes().Where(a => a.Status == status).ToList();
                return View(PaginatedWishList(allwishes, pageNumber, pageSize));
            }
        }

        [Authorize(Roles ="Cinderella")]
        public IActionResult ActiveWishes(int pageNumber, int? pagesize)
        {
            int pageSize = (int)(pagesize == null ? 30 : pagesize);
            var activeWishes = GetWishes().Where(a => a.Status != "Pending" && a.Status != "Received").ToList();
            return View(PaginatedWishList(activeWishes, pageNumber, pageSize));
        }

        [Authorize(Roles = "Cinderella")]
        public IActionResult PendingWishes(int pageNumber, int? pagesize)
        {
            int pageSize = (int)(pagesize == null ? 30 : pagesize);
            var pendingWishes = GetWishes().Where(a => a.Status == "Pending").ToList(); 
            return View(PaginatedWishList(pendingWishes, pageNumber, pageSize));
        }

        [Authorize(Roles = "Cinderella")]
        public IActionResult FulfilledWishes(int pageNumber, int? pagesize)
        {
            int pageSize = (int)(pagesize == null ? 30 : pagesize);
            var FulfilledWishes = GetWishes().Where(a => a.Status == "Received").ToList();
            return View(PaginatedWishList(FulfilledWishes, pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult LoadWishDetails(int wishID)
        {
            try
            {
                var wish = _context.TblWish.Find(wishID);
                return Ok(wish);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        //For Changing Status Pending to Active
        [HttpPost]
        public string SubmitWish(int id)
        {
            var wish = _context.TblWish.Find(id);
            wish!.Status = "Floating in the Wish Clouds";
            wish!.SubmitedDate = DateTime.Now;
            wish!.LastModifiedDate = DateTime.Now;
            _context.Entry(wish).State = EntityState.Modified;
            _context.SaveChanges();
            return "Success";
        }

        [HttpGet]
        public IActionResult GetRandomWishes()
        {
            try
            {
                var wishes = _context.TblWish.Where(a => a.Status == "Floating in the Wish Clouds" && 
                a.CreatedBy != _userManager.GetUserAsync(User).Result.UserName)
                    .OrderBy(c => Guid.NewGuid()).Take(5).ToList();
                return Ok(wishes);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost]
        public string AcceptWish(int id)
        {
            var wishToAccept = _context.TblWish.Find(id);
            wishToAccept!.Status = "Discovered by a Fairy";
            wishToAccept!.Picked_up_date = DateTime.Now;
            wishToAccept!.LastModifiedDate = DateTime.Now;
            wishToAccept!.PickedByFairy = _userManager.GetUserAsync(User).Result.UserName;
            wishToAccept!.PickedByUserId = _userManager.GetUserAsync(User).Result.Id;
            _context.Update(wishToAccept);
            _context.SaveChanges();
            return "Success";
        }

        [HttpPost]
        public string ChangeStatus(int id, string status)
        {
            try
            {
                var wish = _context.TblWish.Find(id);
                wish!.Status = status;
                if (status == "Discovered by a Fairy")
                {
                    wish!.Picked_up_date = DateTime.Now;
                }
                else if (status == "Wish is being Fulfilled") 
                {
                    wish!.FullfilingDate = DateTime.Now;
                }
                else /*if (status == "Fulfilled")*/
                {
                    wish!.CompletedDate = DateTime.Now;
                }
                wish!.LastModifiedDate = DateTime.Now;
                _context.Update(wish);
                _context.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

        [HttpPost]
        public string WishReceived(int id)
        {
            try
            {
                var wish = _context.TblWish.Find(id);
                wish!.Status = "Received";
                wish!.LastModifiedDate = DateTime.Now;
                _context.Update(wish);
                _context.SaveChanges(true);
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }


        //This method for sending wish back to clouds if Fairy is not able to complete the wish
        //Basicall this is for wish cancellation
        [HttpPost]
        public string SentWishtoCloud(int id)
        {
            try
            {
                var wish = _context.TblWish.Find(id);
                wish!.Status = "Floating in the Wish Clouds";
                wish!.CancelledDate = DateTime.Now;
                wish!.IsCancelled = true;
                wish!.PickedByFairy = null;
                wish!.PickedByUserId = null;
                wish!.LastModifiedDate = DateTime.Now;
                _context.Update(wish);
                _context.SaveChanges(true);
                CreateNotification(id, wish!.CreatedByUserId, wish);
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }


        //creating a notification for the cinderella 
        //that will tell her about the cancellation of her wish.
        public void CreateNotification(int id, string? userid, Wishes wish)
        {
            var notification = new Notification
            {
                NotificationTitle = "One of your Discovered Wish ('" + wish.WishText + "') has been Cancelled by a Fairy!",
                DateTime = DateTime.Now,
                WishID = id,
                UserId = userid,
            };
            _context.Notifications.Add(notification);
            _context.SaveChanges(true);
        }


        public IActionResult Notifications()
        {
            var notifications = _context.Notifications
                .Where(a => a.UserId == _userManager.GetUserAsync(User).Result.Id)
                .OrderByDescending(a => a.DateTime).ToList();
            return View(notifications);
        }

        //Mark notification as read
        public string MarknotificationasRead()
        {
            try
            {
                var notifications = _context.Notifications
                    .Where(a => a.UserId == _userManager.GetUserAsync(User).Result.Id
                    && a.MarkasRead == false).ToList();
                foreach (var item in notifications)
                {
                    item.MarkasRead = true;
                    _context.Update(item);
                    _context.SaveChanges();
                }
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }

    }
}
