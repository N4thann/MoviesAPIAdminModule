using Application.DTOs.Request.Studio;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Application.UseCases.Studios.CreateStudio;
using Application.UseCases.Studios.UpdateStudio;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public class StudioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudioController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateStudio([FromBody] CreateStudioRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateStudioCommand(
                request.Name,
                request.CountryName,
                request.CountryCode,
                request.FoundationDate,
                request.History
                );

            var response = await _mediator.Send<CreateStudioCommand, StudioInfoResponse>(command, cancellationToken);

            return CreatedAtAction(nameof(GetStudioById),
                new {id = response.Id },
                response);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudioById(Guid id) => Ok($"Studio with ID {id} details.");

        [HttpPut("{id}/update-basic-info")]
        public async Task<IActionResult> UpdateBasicInfo(Guid id, [FromBody] UpdateBasicInfoStudioRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateBasicInfoStudioCommand(
                id,
                request.Name,
                request.History
                );

            var response = await _mediator.Send<UpdateBasicInfoStudioCommand,StudioInfoResponse>(command, cancellationToken);

            return Ok(response);
        }
    }
}
