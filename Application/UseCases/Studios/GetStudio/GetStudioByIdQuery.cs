using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public record class GetStudioByIdQuery(
        Guid Id
        ) : IQuery<StudioInfoResponse>;
}
