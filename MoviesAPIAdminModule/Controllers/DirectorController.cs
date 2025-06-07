using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    public class DirectorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
