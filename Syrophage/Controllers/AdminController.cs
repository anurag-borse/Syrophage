using Microsoft.AspNetCore.Hosting;
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
        public IActionResult AddOrder()
        {
            var User = unitofworks.User.GetAll().ToList();
            return View(User);
         }

        [HttpPost]
        public IActionResult AddOrder(Order obj , IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var User = unitofworks.User.GetByname(obj.username);


                string wwwRootPath = _webHostEnvironment.WebRootPath;
                
                if ( file != null)
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
                    return RedirectToAction("Dashboard", "Admin");

                }

            }
            TempData["Error"] = "Orer Not Placed";
            return RedirectToAction("AddOrder", "Admin");
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
            return RedirectToAction("ManageOrder" ,"Admin");
        }



    }
}
