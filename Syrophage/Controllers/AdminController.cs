﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Syrophage.Models;
using Syrophage.Repository.IRepository;
using Syrophage.Services;

namespace Syrophage.Controllers
{
    public class AdminController : Controller
    {

        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IUnitofWorks unitofworks, IServices services, IWebHostEnvironment webHostEnvironment)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Dashboard()
        {

            var newslettersCount = unitofworks.Newsletter.GetAll().Count();
            var activeUsersCount = unitofworks.User.GetAll().Where(x => x.IsActivated == true).Count();
            var nonActiveUsersCount = unitofworks.User.GetAll().Where(x => x.IsActivated == false).Count();

            ViewBag.NewslettersCount = newslettersCount;
            ViewBag.ActiveUsersCount = activeUsersCount;
            ViewBag.NonActiveUsersCount = nonActiveUsersCount;

            // var model = new Tuple<List<Product>, List<FeedBack>, List<Enquiry>>(products, feedback, enquiry);


            return View();
        }

        [HttpGet]
        public IActionResult ViewUsers()
        {
            var user = unitofworks.User.GetAll().ToList();
            return View(user);
        }



        [HttpPost]
        public JsonResult ToggleActivation(int id, bool isActivated)
        {
            var user = unitofworks.User.GetById(id);
            if (user != null)
            {
                user.IsActivated = !isActivated;
                unitofworks.User.Update(user);
                unitofworks.Save();


                if (isActivated == false)
                {
                    services.SendActivationEmail(user.Email, user.Password);
                }


                return Json(new { success = true });


            }
            return Json(new { success = false });

        }


        [HttpGet]
        public IActionResult Coupons()
        {
            var coupons = unitofworks.Coupon.GetAll().ToList();
            return View(coupons);
        }

       
        [HttpPost]
        public IActionResult AddCoupon(Coupon coupon)
        {
            if (ModelState.IsValid)
            {

                coupon.IsActivated = false;
                coupon.Code = GenerateCouponCode();
                coupon.CreatedDate = DateTime.Now;

                if(coupon.CouponPicture != null)
                {

                    var file = coupon.CouponPicture;
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CouponImages", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    coupon.CouponPictureUrl = Path.Combine("/CouponImages", fileName).Replace("\\", "/"); ;

                }





                unitofworks.Coupon.Add(coupon);
                unitofworks.Save();
                TempData["Success"] = "Coupon Added Successfully";
                return RedirectToAction("Coupons");
            }
            TempData["Error"] = "Coupon Not Added";
            return RedirectToAction("Coupons");
        }


        public string GenerateCouponCode()
        {
            var random = new Random();
            var code = $"SY{random.Next(100, 999)}RO{random.Next(100, 999)}PA{random.Next(100, 999)}GE";
            return code;
        }

        [HttpPost]
        public JsonResult ToggleActivationCoupon(int id, bool isActivated)
        {
            var coupon = unitofworks.Coupon.GetById(id);
            if (coupon != null)
            {
                coupon.IsActivated = !isActivated;
                unitofworks.Coupon.Update(coupon);
                unitofworks.Save();


                return Json(new { success = true });


            }
            return Json(new { success = false });

        }






        [HttpPost]
        public JsonResult DeleteCoupon(int id)
        {
            var coupon = unitofworks.Coupon.GetById(id);
            if (coupon != null)
            {
                // Get the physical path of the file
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, coupon.CouponPictureUrl.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                unitofworks.Coupon.Remove(coupon);
                unitofworks.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}
