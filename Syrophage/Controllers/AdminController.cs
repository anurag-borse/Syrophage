using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syrophage.Data;
using Syrophage.Migrations;
using Syrophage.Models;
using Syrophage.Models.ViewModel;
using Syrophage.Repository.IRepository;
using Syrophage.Services;

namespace Syrophage.Controllers
{

    public class AdminController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;


        private readonly IWebHostEnvironment _webHostEnvironment;



        public AdminController(IUnitofWorks unitofworks, IServices services, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Dashboard()
        {

            var newslettersCount = unitofworks.Newsletter.GetAll().Count();
            var activeUsersCount = unitofworks.User.GetAll().Where(x => x.IsActivated == true).Count();
            var nonActiveUsersCount = unitofworks.User.GetAll().Where(x => x.IsActivated == false).Count();
            var couponNames = unitofworks.Coupon.GetAll().Select(c => c.Name).ToList();
            var coupondiscounts = unitofworks.Coupon.GetAll().Select(c => c.Discount).ToList();
            ViewBag.CouponNames = couponNames;
            ViewBag.coupondiscounts = coupondiscounts;



            ViewBag.NewslettersCount = newslettersCount;
            ViewBag.ActiveUsersCount = activeUsersCount;
            ViewBag.NonActiveUsersCount = nonActiveUsersCount;




            return View();
        }

        [HttpGet]
        public IActionResult ViewUsers()
        {
            var user = unitofworks.User.GetAll().ToList();
            ViewBag.Coupons = unitofworks.Coupon.GetAll().ToList();

            return View(user);
        }

        [HttpGet]

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["clear"] = "Yor Are Logout :";
            return RedirectToAction("Index", "Home");

        }




        [HttpGet]
        public IActionResult ViewContact()
        {
            var Contacts = unitofworks.Contact.GetAll().ToList();
            return View(Contacts);
        }

        [HttpGet]
        public IActionResult ViewNewsLetter()
        {
            var Newsletter = unitofworks.Newsletter.GetAll().ToList();
            return View(Newsletter);
        }

        [HttpGet]
        public IActionResult ViewTokens()
        {
            var Token = unitofworks.Token.GetAll().ToList();
            return View(Token);
        }


        [HttpGet]
        public IActionResult EditToken(int id)
        {
            var token = unitofworks.Token.GetById(id);
            return View(token);
        }

        [HttpPost]
        public IActionResult EditToken(Token obj)
        {

            var TokeninDb = unitofworks.Token.GetById(obj.Id);


            if (TokeninDb != null)
            {
                TokeninDb.Status = obj.Status;
            }


            unitofworks.Token.Update(TokeninDb);
            unitofworks.Save();

            return RedirectToAction("ViewTokens", "Admin");
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


        [HttpGet]
        public IActionResult AddOrder()
        {
            var User = unitofworks.User.GetAll().ToList();
            return View(User);
        }

        [HttpPost]
        public IActionResult AddOrder(Order obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var User = unitofworks.User.GetByname(obj.username);


                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"OrderImages");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }



                    var order = new Order
                    {
                        ProductName = obj.ProductName,
                        Status = obj.Status,
                        UserId = User.Id,
                        username = obj.username,
                        ProductImageurl = @"\OrderImages\" + filename,
                        date = DateTime.Now,
                        quantity = obj.quantity
                    };




                    unitofworks.Orders.Add(order);
                    unitofworks.Save();

                    TempData["Success"] = "Orer Placed SuccessFully";
                    return RedirectToAction("ManageOrder", "Admin");

                }

            }
            TempData["Error"] = "Orer Not Placed";
            return RedirectToAction("AddOrder", "Admin");
        }

        [HttpPost]
        public IActionResult AddCoupon(Coupon coupon)
        {
            if (ModelState.IsValid)
            {

                coupon.IsActivated = false;
                coupon.Code = services.GenerateCouponCode();
                coupon.CreatedDate = DateTime.Now;

                if (coupon.CouponPicture != null)
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


        [HttpGet]
        public IActionResult ManageOrder()
        {
            var Orders = unitofworks.Orders.GetAll().ToList();
            return View(Orders);
        }

        [HttpGet]
        public IActionResult EidtOrder(int id)
        {
            var Orders = unitofworks.Orders.GetById(id);
            return View(Orders);
        }

        [HttpPost]
        public IActionResult EidtOrder(Order obj)
        {


            var Orerindb = unitofworks.Orders.GetById(obj.id);

            Orerindb.Status = obj.Status;
            Orerindb.ProductName = obj.ProductName;


            unitofworks.Orders.Update(Orerindb);
            unitofworks.Save();

            TempData["Success"] = "Order Updated Succesfully";
            return RedirectToAction("ManageOrder", "Admin");
        }

        [HttpPost]
        public IActionResult AssignCouponToUser(int userId, int couponId)
        {
            var userCoupon = new UserCoupon
            {
                UserId = userId,
                CouponId = couponId
            };

            unitofworks.UserCoupon.Add(userCoupon);
            unitofworks.Save();

            return RedirectToAction("ViewUsers");
        }

        [HttpGet]
        public IActionResult AddCouponToUser(int id)
        {

            var user = unitofworks.User.GetById(id);

            ViewBag.availablecouponsforthisuser = unitofworks.UserCoupon.GetAll().Where(x => x.UserId == id).Select(x => x.CouponId).ToList();



            ViewBag.Coupons = unitofworks.Coupon.GetAll().ToList();

            return View(user);

        }

        [HttpPost]
        public JsonResult RemoveCouponFromUser(int userId, int couponId)
        {
            var userCoupon = unitofworks.UserCoupon.GetAll().FirstOrDefault(uc => uc.UserId == userId && uc.CouponId == couponId);
            if (userCoupon != null)
            {
                unitofworks.UserCoupon.Remove(userCoupon);
                unitofworks.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public JsonResult AddCouponToUser(int userId, int couponId)
        {
            var userCoupon = new UserCoupon
            {
                UserId = userId,
                CouponId = couponId
            };

            unitofworks.UserCoupon.Add(userCoupon);
            unitofworks.Save();

            return Json(new { success = true });
        }





        [HttpGet]
        public IActionResult Categories()
        {
            var categories = unitofworks.Categories.GetAll().ToList();


            return View(categories);
        }


        [HttpPost]
        public IActionResult AddCategory(Categories categories)
        {
            if (ModelState.IsValid)
            {
                if (categories.CategoryPicture != null)
                {

                    var file = categories.CategoryPicture;
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryPictures", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    categories.CategoryPictureUrl = Path.Combine("/CategoryPictures", fileName).Replace("\\", "/"); ;


                }

                unitofworks.Categories.Add(categories);
                unitofworks.Save();

                TempData["Success"] = "Category Added Successfully";
                return RedirectToAction("Categories");
            }


            return View();
        }


        [HttpGet]
        public IActionResult Addproduct()
        {
            var categories = unitofworks.Categories.GetAll().ToList();
            return View(categories);
        }


        [HttpPost]
        public IActionResult Addproduct(Product obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ProductImage");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }



                    var product = new Product
                    {
                        productname = obj.productname,
                        Description = obj.Description,
                        Category = obj.Category,
                        Company = obj.Company,
                        productImageUrl = @"\ProductImage\" + filename
                    };
                    unitofworks.Product.Add(product);
                    unitofworks.Save();

                    TempData["success"] = "product Added";
                    return RedirectToAction("ManageProduct","Admin");

                }
            }

            TempData["Error"] = "product Not Added";
            return RedirectToAction("Addproduct", "Admin");
        }

        [HttpGet]
        public IActionResult ManageProduct()
        {
            var products = unitofworks.Product.GetAll().ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult EidtProduct(int id)
        {
            var products = unitofworks.Product.GetById(id);
            return View(products);
        }


        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            var category = unitofworks.Categories.GetById(id);

            var products = unitofworks.Product.GetAll().Where(j => j.Category == category.CategoryName).ToList();


            foreach(var prod in products)
            {
                unitofworks.Product.Remove(prod);
            }

            if(category != null)
            {
                unitofworks.Categories.Remove(category);
                unitofworks.Save();

                TempData["Success"] = "Category Deleted Successfully";
                return RedirectToAction("Categories", "Admin");
            }


            TempData["Error"] = "Category  not Deleted";
            return RedirectToAction("Categories","Admin");
        }
        [HttpPost]
        public IActionResult EditProduct(Product obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Check if a new image is uploaded
                if (file != null)
                {
                    // Generate a unique filename for the new image
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ProductImage");

                    // Save the new image
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Update product details including the new image URL
                    var product = new Product
                    {
                        id = obj.id, // Assuming Id is the primary key of the product
                        productname = obj.productname,
                        Description = obj.Description,
                        Category = obj.Category,
                        Company = obj.Company,
                        productImageUrl = @"\ProductImage\" + filename
                    };

                    // Remove the old image file if it exists
                    if (!string.IsNullOrEmpty(obj.productImageUrl))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, obj.productImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Update the product in the database
                    unitofworks.Product.Update(product);
                    unitofworks.Save();

                    TempData["success"] = "Product updated successfully.";
                    return RedirectToAction("ManageProduct", "Admin");
                }
                else
                {

                    var product = new Product
                    {
                        id = obj.id,
                        productname = obj.productname,
                        Description = obj.Description,
                        Category = obj.Category,
                        Company = obj.Company,
                        productImageUrl = obj.productImageUrl
                    };

                    // Update the product in the database
                    unitofworks.Product.Update(product);
                    unitofworks.Save();

                    TempData["success"] = "Product updated successfully.";
                    return RedirectToAction("ManageProduct", "Admin");
                }
            }

            TempData["Error"] = "Failed to update product.";
            return RedirectToAction("EditProduct", "Admin", new { id = obj.id }); // Redirect to the edit page with the product ID
        }

    }
}
