using Application.DTOs.Response;
using Application.Interfaces;

namespace Application.Commands.Movie
{
    public record class CreateMovieCommand(
        string Title,
        string OriginalTitle,
        string Synopsis,
        int ReleaseYear,
        int DurationMinutes,
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
        ) : ICommand<MovieInfoBasicResponse>;
}
