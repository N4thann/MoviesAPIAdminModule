
using Application.Commands.Movie;
using Application.Interfaces;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Movies
{
    public class AddAwardUseCase : ICommandHandler<AddAwardCommand>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddAwardUseCase(
            IMovieRepository movieRepository,
            IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddAwardCommand command, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetByIdWithAwardAsync(command.Id);

            if (movie is null)
            {
                throw new KeyNotFoundException($"Movie with ID {command.Id} not found.");
            }

            var award = new Award(command.Name, command.Institution, command.Year);

            movie.AddAward(award);

            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
