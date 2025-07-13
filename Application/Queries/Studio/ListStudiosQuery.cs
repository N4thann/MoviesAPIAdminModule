using Application.Common;
using Application.DTOs.Request.Studio;
using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.Queries.Studio
{
    public record class ListStudiosQuery(
        StudioParametersRequest Parameters) : IQuery<PagedList<StudioInfoResponse>>;
}
