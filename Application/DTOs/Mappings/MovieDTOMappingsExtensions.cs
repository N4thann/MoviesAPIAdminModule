using Application.DTOs.Response;
using Domain.Entities;
using Pandorax.PagedList;

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

        public static IPagedList<MovieBasicInfoResponse> ToMoviePagedListDTO(this IPagedList<Movie> moviesPagedList) 
        {
            if (moviesPagedList == null) throw new InvalidOperationException("Cannot map a null PagedList<Movie> to PagedList<MovieBasicInfoResponse>. The provided 'moviesPagedList' object is null.");

            var movieBasicInfoResponses = moviesPagedList.Select(m => m.ToMovieDTO()).ToList();

            // Usa StaticPagedList para criar um IPagedList<DirectorInfoResponse>
            // Ele recria a lista paginada com os metadados existentes e os novos itens mapeados.
            return new PagedList<MovieBasicInfoResponse>(
                movieBasicInfoResponses!, // O ToMovieDTO pode retornar null, então use ! se tiver certeza que não será null ou adicione tratamento
                moviesPagedList.PageIndex,
                moviesPagedList.PageSize,
                moviesPagedList.TotalItemCount
            );
        }

    }
}
