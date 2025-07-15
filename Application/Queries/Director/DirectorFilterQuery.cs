using Application.Common;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using X.PagedList;

namespace Application.Queries.Director
{
    public record class DirectorFilterQuery(
        string? Name,
        string? CountryName,
        int? AgeBegin,
        int? AgeEnd,
        bool Active,
        QueryStringParameters Parameters
    ) : IQuery<IPagedList<DirectorInfoResponse>>;
}
