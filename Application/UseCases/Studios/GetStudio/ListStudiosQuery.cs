using Application.DTOs.Response.Director;
using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public record class ListStudiosQuery() : IQuery<IEnumerable<StudioInfoResponse>>;
}
