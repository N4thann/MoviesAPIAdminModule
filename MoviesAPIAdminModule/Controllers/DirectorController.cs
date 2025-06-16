using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : Controller
    {
        private readonly IMediator _mediator;
        public DirectorController(IMediator mediator) => _mediator = mediator;
        
        public async Task<IActionResult> CreateDirector()
        {

        } 
    }
}
