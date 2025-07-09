using Application.Common.DTOs;
using Application.DTOs.Request.Studio;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Application.UseCases.Studios.CreateStudio;
using Application.UseCases.Studios.DeleteStudio;
using Application.UseCases.Studios.GetStudio;
using Application.UseCases.Studios.UpdateStudio;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                new {id = response.Id });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(typeof(IEnumerable<StudioInfoResponse>), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista todos os estúdios", Tags = new[] { "Studio Queries" })]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new ListStudiosQuery();
            var response = await _mediator.Query<ListStudiosQuery, IEnumerable<StudioInfoResponse>>(query, cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                   
        [ProducesResponseType(StatusCodes.Status404NotFound)]                   
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Atualiza informações básicas do estúdio", Tags = new[] { "Studio Commands" })]
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

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Atualiza o país de origem de um estúdio", Tags = new[] { "Studio Commands" })]
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

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudioInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Atualiza a data de fundação do estúdio", Tags = new[] { "Studio Commands" })]
        public async Task<IActionResult> UpdateFoundationDate(Guid id, [FromBody] UpdateFoundationStudioRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateFoundationStudioCommand(
                id,
                request.FoundationDate
                );

            var response = await _mediator.Send<UpdateFoundationStudioCommand, StudioInfoResponse>(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Exclui um estúdio por ID", Tags = new[] { "Studio Commands" })]
        public async Task<IActionResult> DeleteStudio(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteStudioCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Ativa um estúdio por ID", Tags = new[] { "Studio Commands" })]
        public async Task<IActionResult> ActivateStudio(Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateStudioCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Desativa um estúdio por ID", Tags = new[] { "Studio Commands" })]
        public async Task<IActionResult> DeactivateStudio(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateStudioCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
