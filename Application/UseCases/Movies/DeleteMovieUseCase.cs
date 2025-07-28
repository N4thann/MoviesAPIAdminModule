using Application.Commands.Movie;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Movies
{
    public class DeleteMovieUseCase : ICommandHandler<DeleteMovieCommand>
    {
        private readonly IRepository<Movie> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMovieUseCase(IRepository<Movie> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = await _repository.GetByIdAsync(command.Id);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {command.Id} not found.");

            _repository.Delete(movie);
            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
