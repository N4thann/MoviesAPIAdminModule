namespace Application.DTOs.Response.Director
{
    public record class DirectorInfoResponse(
        Guid Id,
        string Name,
        DateTime BirthDate,
        string CountryName,
        string CountryCode,
        string? Biography,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        int Age
        );
}
