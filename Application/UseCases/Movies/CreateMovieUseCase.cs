using Application.Commands.Movie;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Validation;
using Domain.ValueObjects;

namespace Application.UseCases.Movies
{
    public class CreateMovieUseCase : ICommandHandler<CreateMovieCommand, MovieBasicInfoResponse>
    {
        private readonly IRepository<Movie> _repositoryMovie;
        private readonly IRepository<Studio> _repositoryStudio;
        private readonly IRepository<Director> _repositoryDirector;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMovieUseCase(IRepository<Movie> repositoryMovie,
            IRepository<Studio> repositoryStudio,
            IRepository<Director> repositoryDirector,
            IUnitOfWork unitOfWork)
        {
            _repositoryMovie = repositoryMovie;
            _repositoryStudio = repositoryStudio;
            _repositoryDirector = repositoryDirector;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieBasicInfoResponse> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var country = new Country(command.CountryName, command.CountryCode);
                var boxOffice = new Money(command.BoxOfficeAmount, command.BoxOfficeCurrency);
                var budget = new Money(command.BudgetAmount, command.BudgetCurrency);
                var genre = new Genre(command.GenreName, command.GenreDescription);
                var duration = new Duration(command.DurationMinutes);

                var studio = await _repositoryStudio.GetByIdAsync(command.StudioId);
                var director = await _repositoryDirector.GetByIdAsync(command.DirectorId);

                if (studio == null)
                    throw new KeyNotFoundException($"Studio with ID {command.StudioId} not found.");

                if (director == null)
                    throw new KeyNotFoundException($"Director with ID {command.DirectorId} not found.");


                var movie = new Movie(
                    command.Title,
                    command.OriginalTitle,
                    command.Synopsis,
                    command.ReleaseYear,
                    duration,
                    country,
                    studio,
                    director,
                    genre,
                    boxOffice,
                    budget
                    );

                _repositoryMovie.Add(movie);
                await _unitOfWork.Commit(cancellationToken);

                var response = movie.ToMovieDTO();

                return response;
            }
            catch (ValidationException ex) 
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }
    }
}
