using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class UpdateCountryStudioCommand(
        string CountryName,
        string CountryCode
        ) : ICommand<StudioInfoResponse>;
}
