namespace Application.DTOs.Request.Movie
{
    public record class CreateMovieRequest(
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
        Guid DirectorId,
        Guid StudioId
        );
}
