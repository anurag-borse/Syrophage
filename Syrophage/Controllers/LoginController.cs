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

            var role = _db.Roles.FirstOrDefault(r => r.email == vm.Email)?.role;

            if (role == null)
            {
                TempData["failed"] = "Login Failed: Invalid Email";
                return View();
            }


            if (role == "Admin")
            {
                // Retrieve the admin based on the email
                var existingAdmin = _db.Admins.SingleOrDefault(u => u.Email == vm.Email);

                if (existingAdmin != null && existingAdmin.Password == vm.Password)
                {
                    // Redirect to admin dashboard
                    return RedirectToAction("Services", "Home");
                }
            }
            else if (role == "User")
            {
                // Retrieve the user based on the email
                var existingUser = _db.Users.SingleOrDefault(u => u.Email == vm.Email);

                if (existingUser != null && existingUser.IsActivated == false)
                {
                    TempData["Error"] = "Not Authorized to Login !!!";
                    return RedirectToAction("Login", "Login");

                }

                if (existingUser != null && existingUser.Password == vm.Password)
                {
                    // Set session variables for user
                    HttpContext.Session.SetInt32("UserId", existingUser.Id);
                    HttpContext.Session.SetString("UserEmail", existingUser.Email);
                    HttpContext.Session.SetString("UserName", existingUser.Name);

                    TempData["Success"] = "Login Successfully";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["failed"] = "Login Failed: Invalid Credentials";
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
                    TempData["repeatemail"] = "Email is Already Exists";
                    // return RedirectToAction("Login", "Login");
                }
                // Check if the phone number already exists
                var existingPhoneUser = _db.Users.FirstOrDefault(r => r.Phone == model.Phone);
                if (existingPhoneUser != null)
                {
                    TempData["repeatephone"] = "Phone no. is Already Exists";
                    return RedirectToAction("Register", "Login");
                }

            if (model.Password != model.ConfirmPassword)
            {
                TempData["Confirm"] = "The new password and confirmed password do not match.";
                return View(model);
            }


            if (model!=null)
            {
                var reg = new Users
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    Phone = model.Phone,
                    IsActivated = false,
                    RegId = GenerateRegId(),
                    Address = "",
                    ProfileImageUrl = ""
                };
                model.IsActivated = false; // Set IsActivated to false when a new user is registered
                _db.Users.Add(reg);
                _db.SaveChanges();

                var role = new Role
                {
                    email = model.Email,
                    role = "User"
                };
                _db.Roles.Add(role);
                _db.SaveChanges();


                //service.SendRegistrationEmail(RL.Registration.Email);
                TempData["Success"] = "Your account has been registered successfully. Please wait for account verification.";
                return RedirectToAction("Index", "Home");
            }
            else
            {

                TempData["Error"] = "Registration Failed";
                return RedirectToAction("Register", "Login");
            }

        }

        public string GenerateRegId()
        {
            // Get the current year
            int year = DateTime.Now.Year;

            // Generate a random 4-digit number
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Generate a 4-digit random number

            // Combine the year and random number to form the registration ID
            string regId = "SP" + year.ToString() + randomNumber.ToString();

            return regId;
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
