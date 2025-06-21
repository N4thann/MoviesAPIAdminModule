using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases.Directors.CreateDirector
{
    public record CreateDirectorCommand(
        string Name,
        DateTime BirthDate,
        string CountryName,
        string CountryCode,
        string? Biography = null,
        Gender Gender = Gender.NotSpecified
    ) : ICommand<DirectorInfoResponse>;
}
