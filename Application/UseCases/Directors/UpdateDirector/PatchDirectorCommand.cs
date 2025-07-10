using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.UseCases.Directors.UpdateDirector
{
    public record class PatchDirectorCommand(
        Guid Id,
        JsonPatchDocument<Director> PatchDocument
        ) : ICommand<DirectorInfoResponse>;
}
