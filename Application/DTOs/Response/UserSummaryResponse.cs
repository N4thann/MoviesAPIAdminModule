namespace Application.DTOs.Response
{
    public record UserSummaryResponse(
    string Id,
    string UserName,
    string Email,
    string? PhoneNumber
);
}
