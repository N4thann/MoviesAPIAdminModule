using Application.DTOs.Common;
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
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                   
        [ProducesResponseType(StatusCodes.Status404NotFound)]                   
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpPut("{id}/update-country-studio")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCountryStudioCommand(
                id,
                request.CountryName,
                request.CountryCode
                );

            var response = await _mediator.Send<UpdateCountryStudioCommand, StudioInfoResponse>(command, cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}/update-foundation-date")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFoundationDate(Guid id, [FromBody] UpdateFoundationStudioRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateFoundationStudioCommand(
                id,
                request.FoundationDate
                );

            var response = await _mediator.Send<UpdateFoundationStudioCommand, StudioInfoResponse>(command, cancellationToken);

            return Ok(response);
        }
    }
}
