namespace Application.DTOs.Response
{
    public record class MovieInfoBasicResponse(
        Guid Id,
        string Name,
        string OriginalTitle,
        string Synopsis,
        int ReleaseYear,
        string DurationToString,
        string CountryName,
        string CountryCode,
        string GenreName,
        string GenreDescription,
        decimal TotalCount,
        decimal BoxOfficeAmount,
        string BoxOfficeCurrency,
        decimal BudgetAmount,
        string BudgetCurrency,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        Guid DirectorId,
        Guid StudioId
        );
}
