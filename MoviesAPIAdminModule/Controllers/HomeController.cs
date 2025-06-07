using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
