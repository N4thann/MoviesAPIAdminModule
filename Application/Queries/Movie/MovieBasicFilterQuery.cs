using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Pandorax.PagedList;

namespace Application.Queries.Movie
{
    public record class MovieBasicFilterQuery(
    string? Title,
    string? OriginalTitle,
    string? CountryName,
    int? ReleaseYearBegin,
    int? ReleaseYearEnd,
    string? DirectorName,
    string? StudioName,
    string? GenreName,
    QueryStringParameters Parameters
    ) : IQuery<IPagedList<MovieBasicInfoResponse>>;
}
