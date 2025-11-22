using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.Queries.Director
{
    public record class DirectorFilterQuery(
        string? Name,
        string? CountryName,
        int? AgeBegin,
        int? AgeEnd,
        bool Active,
        QueryStringParameters Parameters
    ) : IQuery<Result<IPagedList<DirectorInfoResponse>>>;
}
