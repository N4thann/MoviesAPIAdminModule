using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;
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
    ) : IQuery<Result<IPagedList<StudioInfoResponse>>>;
}
