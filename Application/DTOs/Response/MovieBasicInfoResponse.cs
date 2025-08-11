namespace Application.DTOs.Response
{
    public record class MovieBasicInfoResponse(
        Guid Id,
        string Title,
        string OriginalTitle,
        string Synopsis,
        int ReleaseYear,
        string DurationToString,
        string CountryToString,
        string GenreToString,
        string BoxOfficeToString,
        string BudgetToString,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        bool HasPoster,
        bool HasThumbnail,
        int GelleryImagesCount,
        Guid DirectorId,
        Guid StudioId
        );
}
