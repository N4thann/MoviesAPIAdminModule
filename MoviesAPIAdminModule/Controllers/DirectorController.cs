using Application.DTOs.Request.Director;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DirectorController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateDirector([FromBody] CreateDirectorRequest request, CancellationToken cancellationToken)
        {

        } 
    }
}
