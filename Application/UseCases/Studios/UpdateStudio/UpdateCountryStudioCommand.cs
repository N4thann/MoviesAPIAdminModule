using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class UpdateCountryStudioCommand(
        Guid Id,
        string CountryName,
        string CountryCode
        ) : ICommand<StudioInfoResponse>;
}
