using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record PatchStudioCommand(
        Guid Id,
        JsonPatchDocument<Studio> PatchDocument
    ) : ICommand<StudioInfoResponse>;
}
