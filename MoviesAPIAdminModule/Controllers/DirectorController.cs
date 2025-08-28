using Application.Commands.Director;
using Application.Common.Parameters;
using Application.DTOs.Request.Director;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Director;
using Domain.SeedWork.Core;
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
    [Produces("application/json")]
    public class DirectorController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DirectorController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(DirectorInfoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status400BadRequest)]
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

            var result = await _mediator.Send<CreateDirectorCommand, Result<DirectorInfoResponse>>(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(result.Failure);

            var response = result.Success!;

            return CreatedAtAction(nameof(GetById),
                new { id = response.Id },
                response);
        }

        //[HttpPatch("{id}")]
        //[ProducesResponseType(typeof(DirectorInfoResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(Summary = "Atualiza parcialmente um diretor com o JsonPatchDocument", Tags = new[] { "Director Commands" })]
        //public async Task<IActionResult> UpdatePatchDirector(
        //    Guid id,
        //    [FromBody] JsonPatchDocument<Director> patchDoc,
        //    CancellationToken cancellationToken)
        //{
        //    if (patchDoc == null)
        //    {
        //        return BadRequest("Patch document cannot be null.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var command = new PatchDirectorCommand(id, patchDoc);
        //    var response = await _mediator.Send<PatchDirectorCommand, DirectorInfoResponse>(command, cancellationToken);

        //    return Ok(response);
        //}

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DirectorInfoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Obtém um diretor por ID", Tags = new[] { "Director Queries" })]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetDirectorByIdQuery(id);

            var result = await _mediator.Query<GetDirectorByIdQuery, Result<DirectorInfoResponse>>(query, cancellationToken);

            if (result.IsFailure)
                return NotFound(result.Failure);

            var response = result.Success!;

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IPagedList<DirectorInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista todos os diretores aplicando paginação", Tags = new[] { "Director Queries" })]
        public async Task<IActionResult> GetAllPagination([FromQuery] DirectorParametersRequest parameters,CancellationToken cancellationToken)
        {
            var query = new ListDirectorsQuery(parameters);
            var result = await _mediator.Query<ListDirectorsQuery, Result<IPagedList<DirectorInfoResponse>>>(query, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure.Code switch
                {
                    500 => StatusCode(500, result.Failure),
                    _ => BadRequest(result.Failure)
                };
            }

            var response = result.Success!;

            var metadata = new
            {
                response.Count,
                response.PageSize,
                response.PageIndex,
                response.TotalItemCount,
                response.HasNextPage,
                response.HasPreviousPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(response);
        }

        [HttpGet("filtered")]
        [ProducesResponseType(typeof(IPagedList<DirectorInfoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Lista diretores com filtros e paginação", Tags = new[] { "Director Queries" })]
        public async Task<IActionResult> GetFilteredDirectors([FromQuery] DirectorFilterRequest request, CancellationToken cancellationToken)
        {
            var query = new DirectorFilterQuery(
                request.Name,
                request.CountryName,
                request.AgeBegin,
                request.AgeEnd,
                request.Active,
                request
                );

            var result = await _mediator.Query<DirectorFilterQuery, Result<IPagedList<DirectorInfoResponse>>>(query, cancellationToken);

            if (result.IsFailure)
            {
                return result.Failure.Code switch
                {
                    500 => StatusCode(500, result.Failure),
                    _ => BadRequest(result.Failure)
                };
            }
            var response = result.Success!;

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
        [ProducesResponseType(typeof(Failure), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Exclui um diretor por ID", Tags = new[] { "Director Commands" })]
        public async Task<IActionResult> DeleteDirector(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteDirectorCommand(id);

            var result = await _mediator.Send<DeleteDirectorCommand, Result<bool>>(command, cancellationToken);

            if (result.IsFailure)
            {
                if (result.Failure!.Code == 404)
                {
                    return NotFound(result.Failure);
                }
                return BadRequest(result.Failure);
            }

            return NoContent();
        }
    }
}
