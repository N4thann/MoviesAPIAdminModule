using Application.DTOs.Request.Director;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Application.UseCases.Directors.CreateDirector;
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
            var command = new CreateDirectorCommand(
                request.Name,
                request.BirthDate,
                request.CountryName,
                request.CountryCode,
                request.Biography,
                request.Gender
            );

            var response = await _mediator.Send<CreateDirectorCommand, DirectorInfoResponse>(command, cancellationToken);

            return CreatedAtAction(nameof(GetDirectorById),
                new { id = response.Id },
                response);
        }

        [HttpGet("{id}")]
        public IActionResult GetDirectorById(Guid id) => Ok($"Director with ID {id} details.");
    }
}
