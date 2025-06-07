using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
