using Application.DTOs.Response;
using Application.Interfaces;

namespace Application.Queries.Movie
{
    public record class GetMovieByIdQuery(
        Guid Id
        ) : IQuery<MovieInfoBasicResponse>;
}
