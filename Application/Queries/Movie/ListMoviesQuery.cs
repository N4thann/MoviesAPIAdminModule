using Application.DTOs.Request.Movie;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.Queries.Movie
{
    public record class ListMoviesQuery(
        MovieParametersRequest Parameters) : IQuery<Result<IPagedList<MovieBasicInfoResponse>>>;
}
