using Application.DTOs.Request.Director;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Application.UseCases.Directors.CreateDirector;
using Application.UseCases.Directors.DeleteDirector;
using Application.UseCases.Directors.GetDirector;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public class DirectorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DirectorController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(DirectorInfoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Cria um novo diretor", Tags = new[] { "Director Commands" })]
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

            return CreatedAtAction(nameof(GetById),
                new { id = response.Id },
                response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DirectorInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obtém um diretor por ID", Tags = new[] { "Director Queries" })]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetDirectorByIdQuery(id);

            var response = await _mediator.Query<GetDirectorByIdQuery, DirectorInfoResponse>(query, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DirectorInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista todos os diretores", Tags = new[] { "Director Queries" })]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new ListDirectorsQuery();
            var response = await _mediator.Query<ListDirectorsQuery, IEnumerable<DirectorInfoResponse>>(query, cancellationToken);

            return Ok(response);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Exclui um diretor por ID", Tags = new[] { "Director Commands" })]
        public async Task<IActionResult> DeleteDirector(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteDirectorCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

    }
}
