using Microsoft.AspNetCore.Mvc;
using Syrophage.Data;
using Syrophage.Models;
using Microsoft.AspNetCore.Authentication;
using Syrophage.Models.ViewModel;

namespace Syrophage.Controllers
{
    public class LoginController : Controller
    {
        public readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext _db)
        {
            this._db = _db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            var existingUser = _db.Users.SingleOrDefault(u => u.Email == vm.Email);

            if (existingUser != null && existingUser.IsActivated == false)
            {
                TempData["Error"] = "Not Authorized to Login !!!";
                return RedirectToAction("Login", "Login");

            }

            if (existingUser != null)
            {
                if (existingUser.Password == vm.Password)
                {
                    HttpContext.Session.SetInt32("UserId", existingUser.Id);
                    HttpContext.Session.SetString("UserEmail", existingUser.Email);


                    TempData["Success"] = "Login Successfully";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "Login Failed Invalide Creadentials ";
                    return RedirectToAction("Login", "Login");

                }
            }
            return View();
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }






        [HttpPost]
        public IActionResult Register(Users model)
        {
            //var existingEmailUser = unitOfWorks.Users.GetByEmail(usr.User.Email);
            var existingEmail = _db.Users.FirstOrDefault(r => r.Email == model.Email);
            if (existingEmail != null)
            {
                TempData["Error"] = "Email Already Exists";
                return RedirectToAction("Register", "Login");
            }
            // Check if the phone number already exists
            var existingPhoneUser = _db.Users.FirstOrDefault(r => r.Phone == model.Phone);
            if (existingPhoneUser != null)
            {
                TempData["Error"] = "Phone no. Already Exists";
                return RedirectToAction("Register", "Login");
            }
            if (model != null)
            {
                model.IsActivated = false; // Set IsActivated to false when a new user is registered
                _db.Users.Add(model);
                _db.SaveChanges();
                //service.SendRegistrationEmail(RL.Registration.Email);

                return RedirectToAction("Index", "Home");
            }

            TempData["Success"] = "Your account has been registered successfully. Please wait for account verification.";
            return RedirectToAction("Index", "Home");

        }



        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Logout Successfully";
            return RedirectToAction("Index", "Home");
        }




    }
}
