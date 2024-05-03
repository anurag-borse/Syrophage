﻿using Microsoft.AspNetCore.Mvc;
using Syrophage.Data;
using Syrophage.Models;
using Microsoft.AspNetCore.Authentication;
using Syrophage.Models.ViewModel;
using Syrophage.Repository;
using Syrophage.Repository.IRepository;
using Syrophage.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Syrophage.Controllers
{

    public class LoginController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        public LoginController(ApplicationDbContext _db, IUnitofWorks unitofworks, IServices services, IHttpContextAccessor _httpContextAccessor)
        {
            this._db = _db;
            this.unitofworks = unitofworks;
            this.services = services;
            this._httpContextAccessor = _httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            SetLayoutModel();
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {

            var role = _db.Roles.FirstOrDefault(r => r.email == vm.Email)?.role;



            if (role == "Admin")
            {

                // Retrieve the admin based on the email
                var existingAdmin = _db.Admins.SingleOrDefault(u => u.Email == vm.Email);

                if (existingAdmin != null && existingAdmin.Password == vm.Password)
                {
                    HttpContext.Session.SetInt32("AdminId", existingAdmin.Id);
                    HttpContext.Session.SetString("AdminEmail", existingAdmin.Email);


                    //-------------------------------------------------

                    var adminClaims = new List<Claim>
                        {
            new Claim(ClaimTypes.Name, existingAdmin.Email),
            // Add any additional claims specific to admin if needed
                        };

                    var adminClaimsIdentity = new ClaimsIdentity(adminClaims, "AdminAuthentication");

                    var adminAuthProperties = new AuthenticationProperties();

                    HttpContext.SignInAsync("AdminCookieScheme", new ClaimsPrincipal(adminClaimsIdentity), adminAuthProperties);

                    TempData["AdminSuccess"] = "Login Successfully";
                    // Redirect to admin dashboard
                    return RedirectToAction("Dashboard", "Admin");
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
                    HttpContext.Session.SetInt32("UserId", existingUser.Id);
                    // Set session variables for user
                    HttpContext.Session.SetInt32("UserId", existingUser.Id);
                    HttpContext.Session.SetString("UserEmail", existingUser.Email);
                    HttpContext.Session.SetString("UserName", existingUser.Name);
                    //------------------------------------------------------

                    var adminClaims = new List<Claim>
{
    new Claim(ClaimTypes.Name, existingUser.Email),
    // Add any additional claims specific to admin if needed
};

                    var adminClaimsIdentity = new ClaimsIdentity(adminClaims, "UserAuthentication");

                    var adminAuthProperties = new AuthenticationProperties();

                    HttpContext.SignInAsync("AdminCookieScheme", new ClaimsPrincipal(adminClaimsIdentity), adminAuthProperties);




                    TempData["Success"] = "Login Successfully";
                    return RedirectToAction("Index", "Home");
                }
            }



            else
            {
                TempData["Error"] = "Login Failed Invalide Creadentials ";
                return RedirectToAction("Login", "Login");

            }




            TempData["failed"] = "Login Failed: Invalid Credentials";
            return View();
        }





        [HttpGet]
        public IActionResult Register()
        {
            SetLayoutModel();
            return View();
        }






        [HttpPost]
        public IActionResult Register(Users model)
        {
            //var existingEmailUser = unitOfWorks.Users.GetByEmail(usr.User.Email);
            var existingEmail = _db.Users.FirstOrDefault(r => r.Email == model.Email);
            if (existingEmail != null)
            {
                TempData["Message"] = "Email is Already Exists";
                return RedirectToAction("Register", "Login");
            }
            // Check if the phone number already exists
            var existingPhoneUser = _db.Users.FirstOrDefault(r => r.Phone == model.Phone);
            if (existingPhoneUser != null)
            {
                TempData["Message"] = "Phone no. is Already Exists";
                return RedirectToAction("Register", "Login");
            }





            if (model.Password != model.ConfirmPassword)


                if (model != null)
                {
                    var reg = new Users
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword,
                        Phone = model.Phone,
                        IsActivated = false,
                        RegId = services.GenerateRegId(),
                        Address = "",
                        ProfileImageUrl = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png"
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

            TempData["Error"] = "Someting Error Happen";
            return RedirectToAction("Register", "Login");

        }




        [HttpGet]
        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            TempData["clear"] = "Yor Are Logout :";
            return RedirectToAction("Index", "Home");
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

        [HttpGet]

        public IActionResult Forgot()
        {

            return View();
        }



    }
}
