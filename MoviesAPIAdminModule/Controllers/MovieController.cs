using Application.Commands.Movie;
using Application.DTOs.Request.Movie;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Movie;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;
using Newtonsoft.Json;
using Pandorax.PagedList;
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
        [ProducesResponseType(typeof(MovieBasicInfoResponse), StatusCodes.Status201Created)]
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

            var response = await _mediator.Send<CreateMovieCommand, MovieBasicInfoResponse>(command, cancellationToken);

            return CreatedAtAction(nameof(GetByIdBasicInformation),
                new { id = response.Id },
                response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(MovieBasicInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Atualiza parcialmente um filme com o JsonPatchDocument", Tags = new[] { "Movie Commands" })]
        public async Task<IActionResult> UpdatePatchMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchDoc, CancellationToken cancellationToken)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new PatchMovieCommand(id, patchDoc);
            var response = await _mediator.Send<PatchMovieCommand, MovieBasicInfoResponse>(command, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieBasicInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obtém um filme por ID", Tags = new[] { "Movie Queries" })]
        public async Task<IActionResult> GetByIdBasicInformation(Guid id, CancellationToken cancellationToken)
        {

            var command = new GetMovieByIdQuery(id);
            var response = await _mediator.Query<GetMovieByIdQuery, MovieBasicInfoResponse>(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IPagedList<MovieBasicInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista todos os filmes", Tags = new[] { "Movie Queries" })]
        public async Task<IActionResult> GetAllPagination([FromQuery] MovieParametersRequest parameters, CancellationToken cancellationToken)
        {
            var query = new ListMoviesQuery(parameters);
            var response = await _mediator.Query<ListMoviesQuery, IPagedList<MovieBasicInfoResponse>>(query, cancellationToken);

            var metadata = new
            {
                response.Count,
                response.PageSize,
                response.PageIndex,
                response.TotalPageCount,
                response.TotalItemCount,
                response.HasNextPage,
                response.HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("filtered")]
        [ProducesResponseType(typeof(IPagedList<MovieBasicInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista filmes com filtros e paginação", Tags = new[] { "Movie Queries" })]
        public async Task<IActionResult> GetFilteredMovies([FromQuery] MovieBasicFilterRequest request, CancellationToken cancellationToken)
        {
            var query = new MovieBasicFilterQuery(
                request.Title,
                request.OriginalTitle,
                request.CountryName,
                request.ReleaseYearBegin,
                request.ReleaseYearEnd,
                request.DirectorName,
                request.StudioName,
                request.GenreName,
                request
                );

            var response = await _mediator.Query<MovieBasicFilterQuery, IPagedList<MovieBasicInfoResponse>>(query, cancellationToken);

            var metadata = new
            {
                response.Count,
                response.PageSize,
                response.PageIndex,
                response.TotalPageCount,
                response.TotalItemCount,
                response.HasNextPage,
                response.HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Exclui um filme por ID", Tags = new[] { "Movie Commands" })]
        public async Task<IActionResult> DeleteMovie(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteMovieCommand(id);

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("add-award/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Adiciona um prêmio ao filme", Tags = new[] { "Movie Commands" })]
        public async Task<IActionResult> AddAwardToMovie(Guid Id, [FromBody] AwardRequest request, CancellationToken cancellationToken)
        {
            var command = new AddAwardCommand(
                Id,
                request.Name,
                request.Institution,
                request.Year
                );

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("upload-image/{Id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Adiciona uma imagem que pode ser do tipo Poster, Thumbnail ou Gallery ao filme", Tags = new[] { "Movie Commands" })]
        public async Task<IActionResult> UploadImage(Guid Id, [FromForm] UploadImageRequest request, CancellationToken cancellationToken)
        {
            if (request.ImageFile == null || request.ImageFile.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            var command = new AddMovieImageCommand(
                Id,
                request.ImageFile.OpenReadStream(),
                request.ImageFile.FileName,
                request.ImageFile.ContentType,
                request.ImageType,
                request.AltText
            );

            var response = await _mediator.Send<AddMovieImageCommand, string>(command, cancellationToken);

            return Ok(response);
        }
    }
}
