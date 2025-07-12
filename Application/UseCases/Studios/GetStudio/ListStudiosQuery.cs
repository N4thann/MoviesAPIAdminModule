using Application.Common;
using Application.Common.Parameters;
using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public record class ListStudiosQuery(
        StudioParameters Parameters) : IQuery<PagedList<StudioInfoResponse>>;
}
