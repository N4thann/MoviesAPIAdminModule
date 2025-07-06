namespace Application.DTOs.Common
{
    public record class UpdateCountryRequest(
        string CountryName,
        string CountryCode
        );
}
