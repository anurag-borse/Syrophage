using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Syrophage.Models;
using Syrophage.Models.ViewModel;
using Syrophage.Repository.IRepository;
using Syrophage.Services;

namespace Syrophage.Controllers
{
    public class UserController : Controller
    {


        private readonly IUnitofWorks unitofworks;
        private readonly IServices services;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUnitofWorks unitofworks, IServices services, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._webHostEnvironment = _webHostEnvironment;
            this._httpContextAccessor = _httpContextAccessor;
        }


        public IActionResult Profile()
        {
            SetLayoutModel();
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



                TempData["Success"] = "user updated Successfully";
                return RedirectToAction("Profile", "User");
            }


            TempData["Error"] = "user Not Updated";
            return RedirectToAction("Profile", "User");
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
