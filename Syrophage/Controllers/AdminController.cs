using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
using Syrophage.Repository;

namespace Syrophage.Controllers
{
    [Authorize]

    public class AdminController : Controller
    {
        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;


        private readonly IWebHostEnvironment _webHostEnvironment;



        public AdminController(IUnitofWorks unitofworks, IServices services, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._webHostEnvironment = _webHostEnvironment;
        }

        [Authorize]
        public void setAdminData()
        {
            var AdminId = HttpContext.Session.GetInt32("AdminId");
            var Admin = unitofworks.Admin.GetById(AdminId ?? 0);

            ViewData["Admin"] = Admin;
        }




        public IActionResult Dashboard()
        {


            setAdminData();




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

        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            TempData["Success"] = "Yor Are Logout :";

            TempData["clear"] = "Yor Are Logout :";
            return RedirectToAction("Index", "Home");

        }





        /*=============================================Contact us==============================================================*/

        [HttpGet]
        public IActionResult ViewContact()
        {
            setAdminData();

            var Contacts = unitofworks.Contact.GetAll().ToList();
            return View(Contacts);
        }
        /*=============================================Contact us==============================================================*/

        /*=============================================users==============================================================*/

        [HttpGet]
        public IActionResult ViewUsers()
        {
            setAdminData();

            var user = unitofworks.User.GetAll().ToList();
            ViewBag.Coupons = unitofworks.Coupon.GetAll().ToList();

            return View(user);
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



        /*=============================================users==============================================================*/

        /*=============================================NewsLetter==============================================================*/
        [HttpGet]
        public IActionResult ViewNewsLetter()
        {
            setAdminData();

            var Newsletter = unitofworks.Newsletter.GetAll().ToList();
            return View(Newsletter);
        }
        /*=============================================NewsLetter==============================================================*/


        /*=============================================Tokens==============================================================*/
        [HttpGet]
        public IActionResult ViewTokens()
        {
            setAdminData();

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

            TempData["Success"] = "Token Updated Succesfully";
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


                TempData["Success"] = "User Activated Successfully";

                return Json(new { success = true });

            }
            TempData["Error"] = "User Activation Failed";
            return Json(new { success = false });

        }
        /*=============================================Tokens==============================================================*/

        /*=============================================Coupons==============================================================*/
        [HttpGet]
        public IActionResult Coupons()
        {
            setAdminData();

            var coupons = unitofworks.Coupon.GetAll().ToList();
            return View(coupons);
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


                return Json(new { success = true, message = "Activation Done Successfully !!!" });


            }
            return Json(new { success = false, message = "Activation Failed !!!" });

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

                return Json(new { success = true, message = "Coupon Deleted Successfully" });
            }
            return Json(new { success = false, message = "Coupon Not Deleted" });
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
            if (userId >= 0 && couponId >= 0)
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
            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult AddCouponToUser(int id)
        {

            var user = unitofworks.User.GetById(id);

            ViewBag.availablecouponsforthisuser = unitofworks.UserCoupon.GetAll().Where(x => x.UserId == id).Select(x => x.CouponId).ToList();



            ViewBag.Coupons = unitofworks.Coupon.GetAll().ToList();

            return View(user);

        }
        /*=============================================Coupons==============================================================*/


        /*=============================================Order==============================================================*/
        [HttpGet]
        public IActionResult AddOrder()
        {
            setAdminData();

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

                    TempData["Success"] = "Order Placed SuccessFully";
                    return RedirectToAction("ManageOrder", "Admin");

                }

            }
            TempData["Error"] = "Order Not Placed";
            return RedirectToAction("AddOrder", "Admin");
        }


        [HttpGet]
        public IActionResult ManageOrder()
        {
            setAdminData();


            var Orders = unitofworks.Orders.GetAll().ToList();
            return View(Orders);
        }

        [HttpGet]
        public IActionResult EidtOrder(int id)
        {
            setAdminData();

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
        /*=============================================Order==============================================================*/


        /*=============================================Product Categories==============================================================*/
        [HttpGet]
        public IActionResult Categories()
        {
            setAdminData();

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

            TempData["Error"] = "Category Not Added";
            return View();
        }


        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            var category = unitofworks.Categories.GetById(id);
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var products = unitofworks.Product.GetAll().Where(j => j.Category == category.CategoryName).ToList();





            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, category.CategoryPictureUrl.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            foreach (var prod in products)
            {
                unitofworks.Product.Remove(prod);
            }

            if (category != null)
            {
                unitofworks.Categories.Remove(category);
                unitofworks.Save();

                TempData["Success"] = "Category Deleted Successfully";
                return RedirectToAction("Categories", "Admin");
            }


            TempData["Error"] = "Category  not Deleted";
            return RedirectToAction("Categories", "Admin");
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            setAdminData();

            var category = unitofworks.Categories.GetById(id);

            return View(category);
        }


        [HttpPost]
        public IActionResult EditCategory(Categories obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var categories1 = unitofworks.Categories.GetById(obj.Id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;


                if (file != null)
                {
                    string OldImagepath = categories1.CategoryPictureUrl;

                    string oldImagePath = Path.Combine(wwwRootPath, OldImagepath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }



                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"CategoryPictures");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }


                    var category = unitofworks.Categories.GetById(obj.Id);

                    if (category != null)
                    {
                        category.CategoryName = obj.CategoryName;
                        category.CategoryDescription = obj.CategoryDescription;
                        category.CategoryPictureUrl = @"\CategoryPictures\" + filename;
                    }

                    unitofworks.Categories.Update(category);
                    unitofworks.Save();


                    TempData["Success"] = "Category updated successfully";
                    return RedirectToAction("Categories", "Admin");
                }
            }


            TempData["Error"] = "Category  not updated";
            return RedirectToAction("Categories", "Admin");
        }

        /*=============================================Product Categories==============================================================*/


        /*=============================================Service Categories==============================================================*/

        [HttpGet]
        public IActionResult ServiceCategories()
        {
            setAdminData();

            var categories = unitofworks.ServiceCategories.GetAll().ToList();
            return View(categories);
        }
        [HttpPost]
        public IActionResult AddServiceCategory(ServiceCategory obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ServiceCategoryImages");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var cate = new ServiceCategory
                    {
                        CategoryName = obj.CategoryName,
                        CategoryDescription = obj.CategoryDescription,
                        CategoryPictureUrl = @"\ServiceCategoryImages\" + filename
                    };


                    unitofworks.ServiceCategories.Add(cate);
                    unitofworks.Save();

                    TempData["Success"] = "category Added Succesfully";
                    return RedirectToAction("ServiceCategories", "Admin");
                }
            }


            TempData["Error"] = "category Not Added";
            return RedirectToAction("ServiceCategories", "Admin");
        }
        [HttpGet]
        public IActionResult EditServiceCategories(int id)
        {
            setAdminData();

            var categories = unitofworks.ServiceCategories.GetById(id);
            return View(categories);
        }
        [HttpPost]
        public IActionResult EditServiceCategory(ServiceCategory obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var categories1 = unitofworks.ServiceCategories.GetById(obj.Id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {

                    string OldImagepath = categories1.CategoryPictureUrl;

                    string oldImagePath = Path.Combine(wwwRootPath, OldImagepath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }


                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ServiceCategoryImages");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var category = unitofworks.ServiceCategories.GetById(obj.Id);

                    if (category != null)
                    {
                        category.CategoryName = obj.CategoryName;
                        category.CategoryDescription = obj.CategoryDescription;
                        category.CategoryPictureUrl = @"\ServiceCategoryImages\" + filename;
                    }

                    unitofworks.ServiceCategories.Update(category);
                    unitofworks.Save();


                    TempData["Success"] = "Category updated successfully";
                    return RedirectToAction("Servicecategories", "Admin");

                }
            }
            TempData["Error"] = "Category Not updated ";
            return RedirectToAction("Servicecategories", "Admin");
        }
        /*=============================================Service Categories==============================================================*/

        /*=============================================Services==============================================================*/

        [HttpGet]
        public IActionResult AddService()
        {
            setAdminData();

            var categories = unitofworks.ServiceCategories.GetAll().ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult ManageService()
        {
            setAdminData();

            var services = unitofworks.Services.GetAll().ToList();
            return View(services);
        }

        [HttpPost]
        public IActionResult AddService(Service obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var Service = unitofworks.Services.GetById(obj.id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ServicesImages");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var Ser = new Service
                    {
                        servicename = obj.servicename,
                        Description = obj.Description,
                        Category = obj.Category,
                        productImageUrl = @"\ServicesImages\" + filename
                    };


                    unitofworks.Services.Add(Ser);
                    unitofworks.Save();


                    TempData["Success"] = "Service Added Succefully";
                    return RedirectToAction("ManageService", "Admin");

                }

            }
            TempData["Error"] = "Service not Added";
            return RedirectToAction("ManageService", "Admin");
        }

        [HttpGet]
        public IActionResult EidtService(int id)
        {
            setAdminData();

            var services = unitofworks.Services.GetById(id);
            return View(services);
        }

        [HttpPost]
        public IActionResult EditService(Service obj, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                var service1 = unitofworks.Services.GetById(obj.id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {




                    string OldImagepath = service1.productImageUrl;

                    // Remove the old image file if it exists
                    string oldImagePath = Path.Combine(wwwRootPath, OldImagepath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }



                    // Generate a unique filename for the new image
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ServicesImages");

                    // Save the new image
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Update product details including the new image URL

                    var Service = unitofworks.Services.GetById(obj.id);


                    if (Service != null)
                    {
                        Service.servicename = obj.servicename;
                        Service.Category = obj.Category;
                        Service.Description = obj.Description;
                        Service.productImageUrl = @"\ServicesImages\" + filename;

                    }


                    // Update the product in the database
                    unitofworks.Services.Update(Service);
                    unitofworks.Save();

                    TempData["success"] = "Services updated successfully.";
                    return RedirectToAction("ManageService", "Admin");
                }
                else
                {

                    var service = new Service
                    {
                        id = obj.id,
                        servicename = obj.servicename,
                        Description = obj.Description,
                        Category = obj.Category,
                        productImageUrl = obj.productImageUrl
                    };

                    // Update the product in the database
                    unitofworks.Services.Update(service);
                    unitofworks.Save();

                    TempData["success"] = "Service updated successfully.";
                    return RedirectToAction("ManageService", "Admin");
                }

            }
            TempData["Error"] = "Service not Added";
            return RedirectToAction("ManageService", "Admin");
        }
        /*=============================================Services==============================================================*/



        /*=============================================product==============================================================*/
        [HttpGet]
        public IActionResult Addproduct()
        {
            setAdminData();

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
                    return RedirectToAction("ManageProduct", "Admin");

                }
            }

            TempData["Error"] = "product Not Added";
            return RedirectToAction("Addproduct", "Admin");
        }

        [HttpGet]
        public IActionResult ManageProduct()
        {
            setAdminData();

            var products = unitofworks.Product.GetAll().ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult EidtProduct(int id)
        {
            setAdminData();

            var products = unitofworks.Product.GetById(id);
            return View(products);
        }



        [HttpPost]
        public IActionResult EditProduct(Product obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var product1 = unitofworks.Product.GetById(obj.id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Check if a new image is uploaded
                if (file != null)
                {

                    string OldImagepath = product1.productImageUrl;

                    // Remove the old image file if it exists
                    string oldImagePath = Path.Combine(wwwRootPath, OldImagepath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }







                    // Generate a unique filename for the new image
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ProductImage");

                    // Save the new image
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Update product details including the new image URL

                    var product = unitofworks.Product.GetById(obj.id);


                    if (product != null)
                    {
                        product.productname = obj.productname;
                        product.Category = obj.Category;
                        product.Description = obj.Description;
                        product.productImageUrl = @"\ProductImage\" + filename;
                        product.Company = obj.Company;

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


        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            var product = unitofworks.Product.GetById(id);
            if (product != null)
            {
                // Get the physical path of the file
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, product.productImageUrl.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                unitofworks.Product.Remove(product);
                unitofworks.Save();

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }



        [HttpGet]
        public IActionResult MyProfile()
        {
            setAdminData();

            var logedadmin = HttpContext.Session.GetInt32("AdminId");

            var user = unitofworks.Admin.GetById(logedadmin ?? 0);
            return View(user);

        }

        [HttpPost]
        public IActionResult UpdateProfile(Admin admin)
        {
            if (ModelState.IsValid)
            {

                var admininDb = unitofworks.Admin.GetById(admin.Id);

                if (admininDb != null)
                {
                    admininDb.Name = admin.Name;
                    admininDb.Email = admin.Email;
                    admininDb.Contact = admin.Contact;
                    admininDb.Address = admin.Address;
                    admininDb.Password = admin.Password;
                }
                if (admin.ProfileImage != null)
                {
                    if (admininDb.ProfileImageUrl != null)
                    {
                        var filePathProfileImageUrl = Path.Combine(_webHostEnvironment.WebRootPath, admininDb.ProfileImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePathProfileImageUrl))
                        {
                            System.IO.File.Delete(filePathProfileImageUrl);
                        }
                    }

                    var file = admin.ProfileImage;
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AdminProfileImages", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    admininDb.ProfileImageUrl = Path.Combine("/AdminProfileImages", fileName).Replace("\\", "/");

                }

                unitofworks.Admin.Update(admininDb);
                unitofworks.Save();


                TempData["Success"] = "Profile Updated Successfully";
                return RedirectToAction("MyProfile", "Admin");
            }
            TempData["Error"] = "Profile Not Updated";
            return RedirectToAction("MyProfile", "Admin");

        }





    }
}

/*=============================================product==============================================================*/
