using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Movie;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Movies
{
    public class GetMovieByIdUseCase : IQueryHandler<GetMovieByIdQuery, MovieBasicInfoResponse>
    {
        private readonly IRepository<Movie> _repository;

        public GetMovieByIdUseCase(IRepository<Movie> repository) => _repository = repository;

        public async Task<MovieBasicInfoResponse> Handle(GetMovieByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await _repository.GetByIdAsync(query.Id);

                if (movie == null)
                    throw new KeyNotFoundException($"Movie with ID {query.Id} not found.");

                var response = movie.ToMovieDTO();

                return response;

            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }
    }
}
