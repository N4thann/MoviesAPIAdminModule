using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Pandorax.PagedList;

namespace Application.Queries.Studio
{
    public record class StudioFilterQuery(
        string? Name,
        string? CountryName,
        int? FoundationYearBegin,
        int? FoundationYearEnd,
        bool Active,
        QueryStringParameters Parameters
    ) : IQuery<IPagedList<StudioInfoResponse>>;
}
