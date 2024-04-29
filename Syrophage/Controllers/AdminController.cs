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
        public AdminController(IUnitofWorks unitofworks, IServices services)
        {
            this.unitofworks = unitofworks;
            this.services = services;
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


            if (TokeninDb != null) {
                TokeninDb.Status = obj.Status;
                    }


            unitofworks.Token.Update(TokeninDb);
            unitofworks.Save();

            return RedirectToAction("ViewTokens" ,"Admin");
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


        //[HttpGet]
        //public JsonResult ChartData()
        //{
        //    var usersCount = unitofworks.User.GetAll().Count();
        //    var contactsCount = unitofworks.Contact.GetAll().Count();

            
        //}

    }
}
