using Application.DTOs.Response;
using Domain.Entities;
using Domain.SeedWork.Core;
using Pandorax.PagedList;

namespace Application.DTOs.Mappings
{
    public static class MovieDTOMappingsExtensions
    {
        public static Result<MovieBasicInfoResponse>? ToMovieDTO(this Movie movie)
        {
            if (movie == null)
            {
                return Result<MovieBasicInfoResponse>.AsFailure(
                    Failure.Infrastructure("Attempted to map a null Movie entity. This indicates an unexpected error in the application logic."));
            }

            var response = new MovieBasicInfoResponse(
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

            return Result<MovieBasicInfoResponse>.AsSuccess(response);
        }

        public static Result<IPagedList<MovieBasicInfoResponse>> ToMoviePagedListDTO(this IPagedList<Movie> moviesPagedList) 
        {
            if (moviesPagedList == null)
            {
                return Result<IPagedList<MovieBasicInfoResponse>>.AsFailure(
                    Failure.Infrastructure("Cannot map a null PagedList<Movie> to PagedList<MovieInfoResponse>. The provided 'moviesPagedList' object is null."));
            }

            var movieBasicInfoResponses = new List<MovieBasicInfoResponse>();
            foreach (var movie in moviesPagedList)
            {
                var mappingResult = movie.ToMovieDTO();
                if (mappingResult.IsFailure)
                {
                    return Result<IPagedList<MovieBasicInfoResponse>>.AsFailure(mappingResult.Failure!);
                }
                movieBasicInfoResponses.Add(mappingResult.Success!);
            }

            // Usa StaticPagedList para criar um IPagedList<DirectorInfoResponse>
            // Ele recria a lista paginada com os metadados existentes e os novos itens mapeados.
            var response = new PagedList<MovieBasicInfoResponse>(
                movieBasicInfoResponses!, // O ToMovieDTO pode retornar null, então use ! se tiver certeza que não será null ou adicione tratamento
                moviesPagedList.PageIndex,
                moviesPagedList.PageSize,
                moviesPagedList.TotalItemCount
            );

            return Result<IPagedList<MovieBasicInfoResponse>>.AsSuccess(response);
        }

    }
}
