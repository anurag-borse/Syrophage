using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syrophage.Data;
using Syrophage.Repository.IRepository;
using Syrophage.Services;

namespace Syrophage.Controllers
{
    public class AdminController : Controller
    {
        public readonly ApplicationDbContext _db;
        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        
        public AdminController(IUnitofWorks unitofworks, IServices services, ApplicationDbContext _db)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._db = _db; 
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewUsers()
        {
            var user = unitofworks.User.GetAll().ToList();
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

        public IActionResult Coupons(int Id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (user == null)
            {
                return NotFound(); 
            }

           
            return View(user);
           
        }
        

  
    }
}
