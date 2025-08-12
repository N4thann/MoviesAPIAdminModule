using Application.DTOs.Request.Studio;
using Application.DTOs.Response;
using Application.Interfaces;
using Pandorax.PagedList;

namespace Application.Queries.Studio
{
    public record class ListStudiosQuery(
        StudioParametersRequest Parameters) : IQuery<IPagedList<StudioInfoResponse>>;
}
