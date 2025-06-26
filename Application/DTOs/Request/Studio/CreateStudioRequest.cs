namespace Application.DTOs.Request.Studio
{
    public record class CreateStudioRequest(
        string Name,
        string CountryName,
        string CountryCode,
        DateTime FoundationDate,
        string? History = null
        );
}
