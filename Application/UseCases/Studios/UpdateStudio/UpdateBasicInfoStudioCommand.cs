using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class UpdateBasicInfoStudioCommand(
        Guid Id,
        string Name,
        string? History = null
        ) : ICommand<StudioInfoResponse>;
}
