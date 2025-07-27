namespace Application.DTOs.Request.Movie
{
    public record class CreateMovieRequest(
        string Title,
        string OriginalTitle,
        string Synopsis,
        int ReleaseYear,
        int DurationMinutes,
        string CountryName,
        string CountryCode,
        string GenreName,
        string GenreDescription,
        decimal BoxOfficeAmount,
        string BoxOfficeCurrency,
        decimal BudgetAmount,
        string BudgetCurrency,
        Guid DirectorId,
        Guid StudioId
        );
}
