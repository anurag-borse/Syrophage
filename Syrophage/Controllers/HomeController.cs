using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IUnitofWorks unitofworks, IServices services, IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitofworks = unitofworks;
            this.services = services;
            this._httpContextAccessor = _httpContextAccessor;
            this._webHostEnvironment = _webHostEnvironment;
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
            var categorie_product = unitofworks.Product.GetByCategoryName(name);
            return View(categorie_product);
            var carte = unitofworks.Categories.GetAll().ToList();
            return View(carte);
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

        public IActionResult Blogs()
        {
            SetLayoutModel();

            var blogs = unitofworks.Blog.GetAll().Where(blog => blog.IsDisplay == true).ToList();



            return View(blogs);
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
            var services = unitofworks.ServiceCategories.GetAll().ToList();
            return View(services);
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

        [HttpGet]
        public IActionResult ViewServices(string name)
        {
            var services = unitofworks.Services.GetByCategoryName(name).ToList();
            return View(services);
        }


        [HttpPost]
        public IActionResult AddBlog(Blog obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {

                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"BlogImage");

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    DateTime dateTime = DateTime.Now;

                    var blog = new Blog
                    {
                        Name = obj.Name,
                        email = obj.email,
                        BlogDesc = obj.BlogDesc,
                        date = DateOnly.FromDateTime(dateTime),
                        Comments = GenerateNum(),
                        Like = GenerateNum(),
                        ImageUrl = @"\BlogImage\" + filename,
                        Type = obj.Type,
                        Title = obj.Title,
                        IsDisplay = false,
                    };

                    unitofworks.Blog.Add(blog);
                    unitofworks.Save();


                    TempData["Success"] = "Blog Sent to admin";
                    return RedirectToAction("Blogs", "Home");

                }

                TempData["Error"] = "Blog Not Added";
                return RedirectToAction("Blogs", "Home");

            }
            TempData["Error"] = "Blog Not Added";
            return RedirectToAction("Blogs", "Home");
        }




        public static int GenerateNum()
        {
            Random rand = new Random();
            return rand.Next(1, 101); // Generates a random number between 1 and 100
        }

    }
}
