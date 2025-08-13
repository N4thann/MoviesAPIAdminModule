using Application.Common;

namespace Application.DTOs.Request.Movie
{
    public class MovieBasicFilterRequest : QueryStringParameters
    {
        public string? Title { get; init; }
        public string? OriginalTitle { get; init; }
        public string? CountryName { get; init; }
        public int? ReleaseYearBegin { get; init; }
        public int? ReleaseYearEnd { get; init; }
        public string? DirectorName { get; init; }
        public string? StudioName { get; init; }
        public string? GenreName { get; init; }
    }
}
