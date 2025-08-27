using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Movie;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace Application.UseCases.Movies
{
    public class MovieBasicFilterUseCase : IQueryHandler<MovieBasicFilterQuery, Result<IPagedList<MovieBasicInfoResponse>>>
    {
        private readonly IRepository<Movie> _repository;

        public MovieBasicFilterUseCase(IRepository<Movie> repository) => _repository = repository;

        public async Task<Result<IPagedList<MovieBasicInfoResponse>>> Handle(MovieBasicFilterQuery query, CancellationToken cancellationToken)
        {
            var movies = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(query.Title))
                movies = movies.Where(m => m.Name.Contains(query.Title)); //Não precise de StringComparison.OrdinalIgnoreCase nem nada do tipo, o banco de dados já aborda case-insensitive

            if (!string.IsNullOrEmpty(query.OriginalTitle))
                movies = movies.Where(m => m.OriginalTitle.Contains(query.OriginalTitle));

            if (!string.IsNullOrEmpty(query.CountryName))
                movies = movies.Where(m => m.Country.Name.Contains(query.CountryName));

            if (query.ReleaseYearBegin.HasValue)
                movies = movies.Where(m => m.ReleaseYear >= query.ReleaseYearBegin.Value);

            if (query.ReleaseYearEnd.HasValue)
                movies = movies.Where(m => m.ReleaseYear <= query.ReleaseYearEnd.Value);

            if (!string.IsNullOrEmpty(query.DirectorName))
                movies = movies.Where(m => m.Director.Name.Contains(query.DirectorName));

            if (!string.IsNullOrEmpty(query.StudioName))
                movies = movies.Where(m => m.Studio.Name.Contains(query.StudioName));

            if (!string.IsNullOrEmpty(query.GenreName))
                movies = movies.Where(m => m.Genre.Name.Contains(query.GenreName));

            movies = movies.OrderBy(m => m.Name);

            var moviesPaged = await movies.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            return moviesPaged.ToMoviePagedListDTO();
        }
    }
}
