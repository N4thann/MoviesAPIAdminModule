using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class UpdateFoundationStudioCommand(
        DateTime FoundationDate
        ) : ICommand<StudioInfoResponse>;


}
