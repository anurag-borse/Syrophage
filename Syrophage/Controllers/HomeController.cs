using Microsoft.AspNetCore.Mvc;
using Syrophage.Models;
using Syrophage.Models.ViewModel;
using Syrophage.Repository;
using Syrophage.Repository.IRepository;
using Syrophage.Services;
using System.Diagnostics;

namespace Syrophage.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        public HomeController(IUnitofWorks unitofworks, IServices services, IHttpContextAccessor _httpContextAccessor)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public IActionResult Index()
        {
            SetLayoutModel();
            return View();
        }

        public IActionResult Products(Categories categories)
        {
            SetLayoutModel();
            var Allcategories = unitofworks.Categories.GetAll();    
            return View(Allcategories);
        }

        public IActionResult ViewCategorie_Product(string name)
        {
            var categorie_product=unitofworks.Product.GetByCategoryName(name);  
            return View(categorie_product);  
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult About()
        {

            SetLayoutModel();
            return View();
        }

        public IActionResult Contact()
        {
            SetLayoutModel();
            return View();
        }



        //[HttpPost]
        //public IActionResult Products(Categories categories)
        //{
        //    var Allcategories=unitofworks.Categories;
        //    return View();
        //}

        [HttpPost]
        public IActionResult Contact(Contact obj)
        {

            if (ModelState.IsValid)
            {
                unitofworks.Contact.Add(obj);
                unitofworks.Save();

                TempData["Success"] = "Details Sent";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Error occured";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult GContact(Contact obj)
        {

            if (ModelState.IsValid)
            {
                unitofworks.Contact.Add(obj);
                unitofworks.Save();

                TempData["Success"] = "Details Sent";
                return RedirectToAction("Contact", "Home");
            }

            TempData["Error"] = "Error occured";
            return RedirectToAction("Contact", "Home");
        }


        [HttpPost]
        public IActionResult Newsletter(Newsletter obj)
        {

            if (ModelState.IsValid)
            {

                services.SendThanksEmail(obj.email);

                unitofworks.Newsletter.Add(obj);
                unitofworks.Save();

                TempData["Success"] = "Email Suscribed";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Email Falied to Send";
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Services()
        {
            SetLayoutModel();
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}
