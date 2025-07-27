using Application.Commands.Movie;
using Application.DTOs.Request.Movie;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Movie;
using Application.Queries.Studio;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(MovieInfoBasicResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Cria um novo filme", Tags = new[] { "Movie Commands" })]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateMovieCommand(
                request.Title,
                request.OriginalTitle,
                request.Synopsis,
                request.ReleaseYear,
                request.DurationMinutes,
                request.CountryName,
                request.CountryCode,
                request.GenreName,
                request.GenreDescription,
                request.BoxOfficeAmount,
                request.BoxOfficeCurrency,
                request.BudgetAmount,
                request.BudgetCurrency,
                request.DirectorId,
                request.StudioId
                );

            var response = await _mediator.Send<CreateMovieCommand, MovieInfoBasicResponse>(command, cancellationToken);

            return CreatedAtAction(nameof(GetByIdBasicInformation),
                new { id = response.Id },
                response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieInfoBasicResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obtém um filme por ID", Tags = new[] { "Movie Queries" })]
        public async Task<IActionResult> GetByIdBasicInformation(Guid id, CancellationToken cancellationToken)
        {

            var command = new GetMovieByIdQuery(id);
            var response = await _mediator.Query<GetMovieByIdQuery, MovieInfoBasicResponse>(command, cancellationToken);

            return Ok(response);
        }
    }
}
