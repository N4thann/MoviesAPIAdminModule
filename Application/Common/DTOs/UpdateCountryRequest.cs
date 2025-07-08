namespace Application.Common.DTOs
{
    public record class UpdateCountryRequest(
        string CountryName,
        string CountryCode
        );
}
