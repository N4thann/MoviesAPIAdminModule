using Application.Commands.Movie;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Movies
{
    public class DeleteMovieUseCase : ICommandHandler<DeleteMovieCommand, Result<bool>>
    {
        private readonly IRepository<Movie> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMovieUseCase(IRepository<Movie> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteMovieCommand command, CancellationToken cancellationToken)
        {
            var movie = await _repository.GetByIdAsync(command.Id);

            if (movie == null)
                return Result<bool>.AsFailure(Failure.NotFound("Movie", command.Id));

            _repository.Delete(movie);
            await _unitOfWork.Commit(cancellationToken);

            return Result<bool>.AsSuccess(true);
        }
    }
}
