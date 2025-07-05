using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Directors.UpdateDirector
{
    public record class UpdateBasicInfoDirectorCommand(
        Guid Id,
        string Name,
        DateTime NewBirthDate,
        Gender Gender,
        string? Biography
        ) : ICommand<DirectorInfoResponse>;
}
