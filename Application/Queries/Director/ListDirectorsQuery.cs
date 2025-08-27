using Application.Common.Parameters;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.Queries.Director
{
    public record class ListDirectorsQuery(
        DirectorParametersRequest Parameters) : IQuery<Result<IPagedList<DirectorInfoResponse>>>;
}
