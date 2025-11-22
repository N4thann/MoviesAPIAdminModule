using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Application.Queries.Movie;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace Application.UseCases.Movies
{
    public class ListMoviesUseCase : IQueryHandler<ListMoviesQuery, Result<IPagedList<MovieBasicInfoResponse>>>
    {
        private readonly IRepository<Movie> _repository;

        public ListMoviesUseCase(IRepository<Movie> repository) => _repository = repository;

        public async Task<Result<IPagedList<MovieBasicInfoResponse>>> Handle(ListMoviesQuery query, CancellationToken cancellationToken)
        {
            var movies = _repository.GetAllQueryable().OrderBy(p => p.Name);

            var moviesPaged = await movies.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            return moviesPaged.ToMoviePagedListDTO();
        }
    }
}
