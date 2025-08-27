using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Queries.Movie
{
    public record class GetMovieByIdQuery(
        Guid Id
        ) : IQuery<Result<MovieBasicInfoResponse>>;
}
