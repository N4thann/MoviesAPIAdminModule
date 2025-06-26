using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class UpdateBasicInfoCommand(
        string Name,
        string? History = null
        ) : ICommand<StudioInfoResponse>;
}
