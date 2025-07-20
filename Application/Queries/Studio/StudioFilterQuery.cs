using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;

namespace Application.Queries.Studio
{
    public record class StudioFilterQuery(
        string? Name,
        string? CountryName,
        int? FoundationYearBegin,
        int? FoundationYearEnd,
        bool Active,
        QueryStringParameters Parameters
    ) : IQuery<PagedList<StudioInfoResponse>>;
}
