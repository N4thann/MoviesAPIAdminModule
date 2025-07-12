using Application.Common;
using Application.Common.DTOs;
using Application.Common.Parameters;
using Application.DTOs.Request.Studio;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Application.UseCases.Studios.CreateStudio;
using Application.UseCases.Studios.DeleteStudio;
using Application.UseCases.Studios.GetStudio;
using Application.UseCases.Studios.UpdateStudio;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public class StudioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudioController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Cria um novo estúdio", Tags = new[] { "Studio Commands" })]
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

            return CreatedAtAction(nameof(GetById),
                new {id = response.Id },
                response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obtém um estúdio por ID", Tags = new[] { "Studio Queries" })]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) 
        {
            var command = new GetStudioByIdQuery(id);
            var response = await _mediator.Query<GetStudioByIdQuery, StudioInfoResponse>(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<StudioInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista todos os estúdios", Tags = new[] { "Studio Queries" })]
        public async Task<IActionResult> GetAllPagination([FromQuery] StudioParameters parameters, CancellationToken cancellationToken)
        {
            var query = new ListStudiosQuery(parameters);
            var response = await _mediator.Query<ListStudiosQuery, PagedList<StudioInfoResponse>>(query, cancellationToken);

            var metadata = new
            {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.TotalPages,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Atualiza parcialmente um estúdio com o JsonPatchDocument", Tags = new[] { "Studio Commands" })]
        public async Task<IActionResult> UpdatePatchStudio(
            Guid id,
            [FromBody] JsonPatchDocument<Studio> patchDoc,
            CancellationToken cancellationToken)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new PatchStudioCommand(id, patchDoc);
            var response = await _mediator.Send<PatchStudioCommand, StudioInfoResponse>(command, cancellationToken);
            
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Exclui um estúdio por ID", Tags = new[] { "Studio Commands" })]
        public async Task<IActionResult> DeleteStudio(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteStudioCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
