using Microsoft.AspNetCore.Mvc;

namespace Syrophage.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
