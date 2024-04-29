using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Syrophage.Models;
using Syrophage.Repository.IRepository;
using Syrophage.Services;

namespace Syrophage.Controllers
{
    public class UserController : Controller
    {


        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUnitofWorks unitofworks, IServices services, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._webHostEnvironment = _webHostEnvironment;
        }


        public IActionResult Profile()
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? -1;


            var user = unitofworks.User.GetById(userId);
            return View(user);
        }


        [HttpPost]
        public IActionResult updateUser( Users obj , IFormFile file)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"UserProfilePic");



                using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var user = unitofworks.User.GetById(obj.Id);

                user.Name = obj.Name;
                user.Email = obj.Email;
                user.Phone = obj.Phone;
                user.Address = obj.Address;
                user.ProfileImageUrl = @"\UserProfilePic\" + filename;


                unitofworks.User.Update(user);
                unitofworks.Save();

                ViewBag["ProfileImage"] = user.ProfileImageUrl;


                TempData["Success"] = "user updated Successfully";
                return RedirectToAction("Profile", "User");
            }


            TempData["Error"] = "user Not Updated";
            return RedirectToAction("Profile", "User");
        }
        
    }
}
