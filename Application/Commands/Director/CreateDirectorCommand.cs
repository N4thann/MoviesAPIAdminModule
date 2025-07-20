using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Commands.Director
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
