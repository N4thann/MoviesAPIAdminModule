using Application.DTOs.Response.Director;
using Application.Interfaces;

namespace Application.UseCases.Directors.UpdateDirector
{
    public record class UpdateCountryDirectorCommand(
        Guid Id,
        string CountryName,
        string CountryCode
        ) : ICommand<DirectorInfoResponse>;
}
