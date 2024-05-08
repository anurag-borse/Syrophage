using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Syrophage.Models;
using Syrophage.Models.ViewModel;
using Syrophage.Repository.IRepository;
using Syrophage.Services;

namespace Syrophage.Controllers
{
    [Authorize]
    public class UserController : Controller
    {


        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUnitofWorks unitofworks, IServices services, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._webHostEnvironment = _webHostEnvironment;
            this._httpContextAccessor = _httpContextAccessor;
        }

       
        public IActionResult Profile()
        {
            SetLayoutModel();
            int userId = HttpContext.Session.GetInt32("UserId") ?? -1;


            var user = unitofworks.User.GetById(userId);
            return View(user);
        }


        [HttpPost]
        public IActionResult updateUser(Users obj, IFormFile file)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var user1 = unitofworks.User.GetByemail(obj.Email);
            if (file == null)
            {
               
                var user = unitofworks.User.GetById(obj.Id);

                user.Name = obj.Name;
                user.Email = obj.Email;
                user.Phone = obj.Phone;
                user.Address = obj.Address;
                user.ProfileImageUrl = user1.ProfileImageUrl;


                unitofworks.User.Update(user);
                unitofworks.Save();



                TempData["Success"] = "user updated Successfully";
                return RedirectToAction("Profile", "User");
            }

            else
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"UserProfilePic");



                using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                var user = unitofworks.User.GetById(obj.Id);

                user.Name = obj.Name;
                user.Email = obj.Email;
                user.Phone = obj.Phone;
                user.Address = obj.Address;
                user.ProfileImageUrl = @"\UserProfilePic\" + filename;


                unitofworks.User.Update(user);
                unitofworks.Save();

                TempData["Success"] = "user updated Successfully";
                return RedirectToAction("Profile", "User");

            }
        }



        [HttpGet]
        public IActionResult TokenGenerate()
        {
            SetLayoutModel();
            int userId = HttpContext.Session.GetInt32("UserId") ?? -1;

            var user = unitofworks.User.GetById(userId);
            var tokens = unitofworks.Token.GetAll(u => u.RegId == user.RegId).OrderByDescending(c => c.Id).ToList();

            var model = new Tuple<Users, List<Token>>(user, tokens);


            return View(model);

        }

        [HttpPost]
        public IActionResult TokenGenerate(Token token)
        {
            if (ModelState.IsValid)
            {
                token.RegId = token.RegId;
                token.RequestId = services.GenerateTokenId();
                token.Date = DateTime.Now;
                token.Status = "Pending";
                token.Name = token.Name;
                token.RequestQuery = token.RequestQuery;

                if (token.Attachment1 != null)
                {
                    var file = token.Attachment1;
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    token.Attachment1Url = Path.Combine("/Db_Images", fileName).Replace("\\", "/"); ;

                }

                if (token.Attachment2 != null)
                {
                    var file = token.Attachment2;
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    token.Attachment2Url = Path.Combine("/Db_Images", fileName).Replace("\\", "/"); ;

                }


                unitofworks.Token.Add(token);
                unitofworks.Save();
                TempData["Success"] = "Token Generated Successfully";
                return RedirectToAction("TokenGenerate", "User");
            }
            TempData["Error"] = "Token Not Generated";
            return RedirectToAction("TokenGenerate", "User");

        }




        public void SetLayoutModel()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var user = unitofworks.User.GetById(userId);


            if (userId != 0)
            {


                var layoutModel = new LayoutVM
                {

                    FirstName = user.Name,
                    LastName = user.Email,
                    profilepic = user.ProfileImageUrl // Use the null-conditional operator to avoid NullReferenceException
                };
                // If Profilepic is null, set a default image or leave it as null

                _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;
            }
        }

        [HttpGet]
        public IActionResult Orders()
        {

            SetLayoutModel();
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var orders = unitofworks.Orders.GetByUserID(userId);
            return View(orders);
        }


        [HttpGet]
        public IActionResult Trackorder(int id)
        {
            SetLayoutModel();
            var order = unitofworks.Orders.GetById(id);
            ViewBag.OrderStatus = order.Status;
            return View(order);
        }





        [HttpGet]

        public IActionResult TotalU()
        {
            int totalUserCount = GetUserCountFromDatabase();

            return Json(new { totalUsers = totalUserCount });
        }
        private int GetUserCountFromDatabase()
        {
            // Replace this with your actual logic to fetch total user count from your database
            // For example, using Entity Framework or any other data access method
            return unitofworks.User.Count();
        }




        [HttpGet]

        public IActionResult TotalCoupons()
        {
            int totalCouponCount = GetCouponCountFromDatabase();

            return Json(new { totalCoupons = totalCouponCount });
        }
        private int GetCouponCountFromDatabase()
        {
            // Replace this with your actual logic to fetch total user count from your database
            // For example, using Entity Framework or any other data access method
            return unitofworks.Coupon.Count();
        }



        [HttpGet]

        public IActionResult TotalNews()
        {
            int totalCouponCount = GetNewsCountFromDatabase();

            return Json(new { totalNews = totalCouponCount });
        }
        private int GetNewsCountFromDatabase()
        {
            // Replace this with your actual logic to fetch total user count from your database
            // For example, using Entity Framework or any other data access method
            return unitofworks.Newsletter.Count();
        }


        [HttpGet]

        public IActionResult TotalContact()
        {
            int totalCouponCount = GetContactCountFromDatabase();

            return Json(new { totalContact = totalCouponCount });
        }
        private int GetContactCountFromDatabase()
        {
            // Replace this with your actual logic to fetch total user count from your database
            // For example, using Entity Framework or any other data access method
            return unitofworks.Contact.Count();
        }










        [HttpGet]
        public IActionResult Coupons()
        {
            SetLayoutModel();

            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;


            ViewBag.UserId = userId;




            var userCoupons = unitofworks.UserCoupon.GetAll(u => u.UserId == userId, includeProperties: "Coupon").ToList();

            var coupons = userCoupons.Select(uc => uc.Coupon).ToList();

            // Pass the list of Coupon objects to the view
            return View(coupons);

        }

     

    }
}
