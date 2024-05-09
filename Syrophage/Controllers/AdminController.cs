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
using Syrophage.Repository.IRepository;
using Syrophage.Services;
using Syrophage.Repository;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Rotativa.AspNetCore;
using Newtonsoft.Json;
using Syrophage.Models.ViewModel;
using System.Runtime.ConstrainedExecution;

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


        [HttpPost]
        public JsonResult ToggleActivation1(int id, bool isActivated)
        {
            var blog = unitofworks.Blog.GetById(id);
            if (blog != null)
            {
                blog.IsDisplay = !isActivated;
                unitofworks.Blog.Update(blog);
                unitofworks.Save();






                return Json(new { success = true });

            }
            TempData["Error"] = "Blog did not Displayed";
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

            setAdminData();

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

                var Cate = new MailVm
                {
                    ProductCatVm = categories
                };

                var users = unitofworks.Newsletter.GetAll();

                foreach (var user in users)
                {
                    services.SendJobAddedEmail(user.email, Cate);
                }


                TempData["Success"] = "Category Added Successfully";
                return RedirectToAction("Categories");
            }

            TempData["Error"] = "Category Not Added";
            return RedirectToAction("Categories");
        }


        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            var category = unitofworks.Categories.GetById(id);

            if (category.CategoryPictureUrl != null)
            {
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
        public IActionResult EditCategory(Categories category)
        {

            if (category != null)
            {
                var categoryinDb = unitofworks.Categories.GetById(category.Id);

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (category.CategoryPicture != null)
                {

                    if (categoryinDb.CategoryPictureUrl != null)
                    {
                        string OldImagepath = categoryinDb.CategoryPictureUrl;
                        string oldImagePath = Path.Combine(wwwRootPath, OldImagepath.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(category.CategoryPicture.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"CategoryPictures");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        category.CategoryPicture.CopyTo(fileStream);
                    }

                    category.CategoryPictureUrl = Path.Combine("/CategoryPictures", filename).Replace("\\", "/"); ;

                }
                if (category != null)
                {
                    categoryinDb.CategoryName = category.CategoryName;
                    categoryinDb.CategoryDescription = category.CategoryDescription;
                    categoryinDb.CategoryPictureUrl = category.CategoryPictureUrl;
                }

                unitofworks.Categories.Update(categoryinDb);
                unitofworks.Save();

                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Categories", "Admin");

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

        [HttpGet]
        public IActionResult DeleteServiceCategory(int id)
        {
            var category = unitofworks.ServiceCategories.GetById(id);
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            var services = unitofworks.Services.GetAll().Where(j => j.Category == category.ServiceCategoryName).ToList();

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, category.ServiceCategoryPictureUrl.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            foreach (var prod in services)
            {
                unitofworks.Services.Remove(prod);
            }

            if (category != null)
            {
                unitofworks.ServiceCategories.Remove(category);
                unitofworks.Save();

                TempData["Success"] = "Category Deleted Successfully";
                return RedirectToAction("ServiceCategories", "Admin");
            }

            TempData["Error"] = "Category  not Deleted";
            return RedirectToAction("ServiceCategories", "Admin");
        }


        [HttpPost]
        public IActionResult AddServiceCategory(ServiceCategory obj)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (obj.ServiceCategoryPicture != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.ServiceCategoryPicture.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ServiceCategoryImages");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        obj.ServiceCategoryPicture.CopyTo(fileStream);
                    }

                    var cate = new ServiceCategory
                    {
                        ServiceCategoryName = obj.ServiceCategoryName,
                        ServiceCategoryDescription = obj.ServiceCategoryDescription,
                        ServiceCategoryPictureUrl = @"\ServiceCategoryImages\" + filename
                    };


                    unitofworks.ServiceCategories.Add(cate);
                    unitofworks.Save();

                    var Cate = new MailVm
                    {
                        SerCatVm = cate
                    };

                    var users = unitofworks.Newsletter.GetAll();

                    foreach (var user in users)
                    {
                        services.SendJobAddedEmail(user.email, Cate);
                    }


                    TempData["Success"] = "Category Added Succesfully";
                    return RedirectToAction("ServiceCategories", "Admin");
                }
            }


            TempData["Error"] = "Category Not Added";
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
        public IActionResult EditServiceCategory(ServiceCategory obj)
        {

            if (obj != null)
            {
                var categoryinDb = unitofworks.ServiceCategories.GetById(obj.Id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (obj.ServiceCategoryPicture != null)
                {
                    if (categoryinDb.ServiceCategoryPictureUrl != null)
                    {
                        string OldImagepath = categoryinDb.ServiceCategoryPictureUrl;
                        string oldImagePath = Path.Combine(wwwRootPath, OldImagepath.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.ServiceCategoryPicture.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"ServiceCategoryImages");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        obj.ServiceCategoryPicture.CopyTo(fileStream);
                    }

                    obj.ServiceCategoryPictureUrl = Path.Combine("/ServiceCategoryImages", filename).Replace("\\", "/");

                }

                if (obj != null)
                {
                    categoryinDb.ServiceCategoryName = obj.ServiceCategoryName;
                    categoryinDb.ServiceCategoryDescription = obj.ServiceCategoryDescription;
                    categoryinDb.ServiceCategoryPictureUrl = categoryinDb.ServiceCategoryPictureUrl;
                }

                unitofworks.ServiceCategories.Update(categoryinDb);
                unitofworks.Save();


                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Servicecategories", "Admin");

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


                    var Cate = new MailVm
                    {
                        Servicevm = Ser
                    };

                    var users = unitofworks.Newsletter.GetAll();

                    foreach (var user in users)
                    {
                        services.SendJobAddedEmail(user.email, Cate);
                    }



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

            if (obj != null)
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

                    TempData["Success"] = "Services updated successfully.";
                    return RedirectToAction("ManageService", "Admin");
                }
                else
                {

                    var Service = unitofworks.Services.GetById(obj.id);

                    if (Service != null)
                    {
                        Service.servicename = obj.servicename;
                        Service.Category = obj.Category;
                        Service.Description = obj.Description;
                        Service.productImageUrl = service1.productImageUrl;

                    }

                    // Update the product in the database
                    unitofworks.Services.Update(Service);
                    unitofworks.Save();

                    TempData["Success"] = "Service updated successfully.";
                    return RedirectToAction("ManageService", "Admin");
                }

            }
            TempData["Error"] = "Service not Added";
            return RedirectToAction("ManageService", "Admin");
        }



        [HttpGet]
        public IActionResult DeleteService(int id)
        {
            var service = unitofworks.Services.GetById(id);
            if (service != null)
            {
                // Get the physical path of the file
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, service.productImageUrl.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                unitofworks.Services.Remove(service);
                unitofworks.Save();


                TempData["Success"] = "Service Deleted Succesfully";
                return RedirectToAction("ManageService", "Admin");
            }
            TempData["Error"] = "Service Not Deleted";
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


                    var Cate = new MailVm
                    {
                        Productvm = product
                    };

                    var users = unitofworks.Newsletter.GetAll();

                    foreach (var user in users)
                    {
                        services.SendJobAddedEmail(user.email, Cate);
                    }


                    TempData["Success"] = "product Added";
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
            if (obj != null)
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

                    TempData["Success"] = "Product updated successfully.";
                    return RedirectToAction("ManageProduct", "Admin");
                }
                else
                {
                    var product = unitofworks.Product.GetById(obj.id);


                    if (product != null)
                    {
                        product.productname = obj.productname;
                        product.Category = obj.Category;
                        product.Description = obj.Description;
                        product.productImageUrl = product1.productImageUrl;
                        product.Company = obj.Company;

                    }

                    // Update the product in the database
                    unitofworks.Product.Update(product);
                    unitofworks.Save();

                    TempData["Success"] = "Product updated successfully.";
                    return RedirectToAction("ManageProduct", "Admin");
                }
            }

            TempData["Error"] = "Failed to update product.";
            return RedirectToAction("EidtProduct", "Admin", new { id = obj.id }); // Redirect to the edit page with the product ID
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

        /*=============================================product==============================================================*/


        /*=============================================Quatation==============================================================*/
        [HttpGet]
        public IActionResult Cotations()
        {
            setAdminData();
            var Quatation = unitofworks.QuatationFix.GetAll().ToList();

            var Admin = unitofworks.Admin.GetAll().ToList();
            var model = new Tuple<List<Quatation_details_fix>, List<Admin>>(Quatation, Admin);

            return View(model);
        }

        [HttpGet]
        public IActionResult SendQuatationMail()
        {
            setAdminData();

            return View();
        }

        [HttpPost]
        public IActionResult SendQuatationMail(QuatationVm obj)
        {

            if (obj != null)
            {
                var Quatationdata = unitofworks.QuaForm.GetAlll()
             .Include(q => q.Services) // Include Services
             .FirstOrDefault();// Assuming you only need one QuatationFormData

                if (Quatationdata != null)
                {
                    // Create a QuotationFormData object
                    var Data = new QuotationFormData
                    {
                        CompanyName = Quatationdata.CompanyName,
                        QuotationBy = Quatationdata.QuotationBy,
                        PreparedBy = Quatationdata.PreparedBy,
                        Role = Quatationdata.Role,
                        Contact = Quatationdata.Contact,
                        Email = Quatationdata.Email,
                        PreparedFor = Quatationdata.PreparedFor,
                        ContactTo = Quatationdata.ContactTo,
                        EmailTo = Quatationdata.EmailTo,
                        AboutUs = Quatationdata.AboutUs,
                        Methodology = Quatationdata.Methodology,
                        Expectation = Quatationdata.Expectation,
                        Term1 = Quatationdata.Term1,
                        Term2 = Quatationdata.Term2,
                        Term3 = Quatationdata.Term3,
                        Services = Quatationdata.Services // Assuming Services is a list of ServiceData
                    };

                    var report = new ViewAsPdf("GenerateQuotationPDF", Data)
                    {
                        FileName = "UnprotectedReport.pdf"
                    };

                    var binaryPdf = report.BuildFile(ControllerContext).Result;

                    var stream = new MemoryStream(binaryPdf);
                    string Subject = "Product Invoice";
                    string Body = obj.Description;

                    bool emailSent = services.SendQuotationEmail(obj.Email, Subject, Body, stream, "Attachment.pdf");


                    if (emailSent)
                    {
                        unitofworks.QuaForm.Remove(Quatationdata);
                        unitofworks.Save();

                        TempData["Success"] = "Qutation Sent";
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        TempData["Error"] = "Some Error Occured";
                        return RedirectToAction("Dashboard", "Admin");
                    }
                }
            }

            TempData["Error"] = "Some Error Occured";
            return RedirectToAction("Dashboard", "Admin");
        }



        [HttpPost]
        public JsonResult GenerateQuotationPDF([FromBody] QuotationFormData formData)
        {
            try
            {
                // Deserialize the JSON string back to a list of ServiceData objects
                string servicesJson = JsonConvert.SerializeObject(formData.Services);


                var quotationEntity = new QuotationFormData
                {
                    QuotationBy = formData.QuotationBy,
                    PreparedBy = formData.PreparedBy,
                    Role = formData.Role,
                    Contact = formData.Contact,
                    Email = formData.Email,
                    CompanyName = formData.CompanyName,
                    PreparedFor = formData.PreparedFor,
                    ContactTo = formData.ContactTo,
                    EmailTo = formData.EmailTo,
                    AboutUs = formData.AboutUs,
                    Methodology = formData.Methodology,
                    Expectation = formData.Expectation,
                    Services = formData.Services, // Serialize the list of services
                    Term1 = formData.Term1,
                    Term2 = formData.Term2,
                    Term3 = formData.Term3
                };



                unitofworks.QuaForm.Add(quotationEntity);
                unitofworks.Save();


                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }



        [HttpGet]
        public IActionResult AllBlog()
        {
            setAdminData();

            var blogs = unitofworks.Blog.GetAll().ToList();
            return View(blogs);

        }


    }
}






