using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        [HttpGet]
        public IActionResult ViewUsers()
        {
            var user = unitofworks.User.GetAll().ToList();
            return View(user);
        }
  
    }
}
