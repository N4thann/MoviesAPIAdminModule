using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    public class StudioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
