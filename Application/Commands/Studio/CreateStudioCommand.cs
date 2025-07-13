using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.Commands.Studio
{
    public record class CreateStudioCommand(
        string Name,
        string CountryName,
        string CountryCode,
        DateTime FoundationDate,
        string? History = null
        ) : ICommand<StudioInfoResponse>;
}
