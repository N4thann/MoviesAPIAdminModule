using Application.DTOs.Response;
using Domain.Entities;

namespace Application.DTOs.Mappings
{
    public static class MovieDTOMappingsExtensions
    {
        public static MovieBasicInfoResponse? ToMovieDTO(this Movie movie)
        {
            if (movie == null) throw new InvalidOperationException($"Cannot map a null {typeof(Movie)} entity to {typeof(DirectorInfoResponse)}. The provided 'director' object is null.");

            return new MovieBasicInfoResponse(
                movie.Id,
                movie.Name,
                movie.OriginalTitle,
                movie.Synopsis,
                movie.ReleaseYear,
                movie.Duration.ToString(),
                movie.Country.ToString(),
                movie.Genre.ToString(),
                movie.BoxOffice.ToString(),
                movie.Budget.ToString(),
                movie.CreatedAt,
                movie.UpdatedAt,
                movie.HasPoster,
                movie.HasThumbnail,
                movie.GalleryImagesCount,
                movie.DirectorId,
                movie.StudioId
                );
        }
    }
}
