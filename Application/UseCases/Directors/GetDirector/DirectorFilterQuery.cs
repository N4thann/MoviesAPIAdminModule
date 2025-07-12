using Application.Common;
using Application.DTOs.Response.Director;
using Application.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public record class DirectorFilterQuery(
        string? Name,
        string? CountryName,
        int? AgeBegin,
        int? AgeEnd,
        bool Active,
        QueryStringParameters Parameters
    ) : IQuery<PagedList<DirectorInfoResponse>>;
}
