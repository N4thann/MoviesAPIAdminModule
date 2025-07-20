using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Commands.Director
{
    public record class PatchDirectorCommand(
        Guid Id,
        JsonPatchDocument<Domain.Entities.Director> PatchDocument
        ) : ICommand<DirectorInfoResponse>;
}
