namespace Application.DTOs.Response
{
    public record class MovieInfoBasicResponse(
        Guid Id,
        string Title,
        string OriginalTitle,
        string Synopsis,
        int ReleaseYear,
        string DurationToString,
        string Country,
        string Genre,//teste de genre e country com to string
        decimal? BoxOfficeAmount,
        string BoxOfficeCurrency,
        decimal? BudgetAmount,
        string BudgetCurrency,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        bool HasPoster,
        bool HasThumbnail,
        int GelleryImagesCount,
        Guid DirectorId,
        Guid StudioId
        );
}
