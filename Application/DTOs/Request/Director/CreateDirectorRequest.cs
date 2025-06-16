using Domain.Entities;

namespace Application.DTOs.Request.Director
{
    public record class CreateDirectorRequest(
        string Name,
        DateTime BirthDate,
        string CountryName,
        string CountryCode, 
        string? Biography = null,
        Gender Gender = Gender.NotSpecified
        );
}
