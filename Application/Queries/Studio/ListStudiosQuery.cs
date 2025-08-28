using Application.DTOs.Request.Studio;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.Queries.Studio
{
    public record class ListStudiosQuery(
        StudioParametersRequest Parameters) : IQuery<Result<IPagedList<StudioInfoResponse>>>;
}
