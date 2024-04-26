using Microsoft.AspNetCore.Mvc;
using Syrophage.Models;
using Syrophage.Repository.IRepository;
using System.Diagnostics;

namespace Syrophage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitofWorks unitofworks;
        public HomeController(IUnitofWorks unitofworks)
        {
            this.unitofworks = unitofworks;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


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




        public IActionResult Token()
        {
            return View();
        }










        public IActionResult Services()
        {
            return View();
        }
        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
