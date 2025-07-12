using Application.Common;
using Application.DTOs.Request.Studio;
using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public record class ListStudiosQuery(
        StudioParametersRequest Parameters) : IQuery<PagedList<StudioInfoResponse>>;
}
