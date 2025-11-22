using Application.Commands.Director;
using Application.Interfaces.Mediator;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors
{
    public class DeleteDirectorUseCase : ICommandHandler<DeleteDirectorCommand, Result<bool>>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteDirectorCommand command, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(command.Id);

            if (director == null)
                return Result<bool>.AsFailure(Failure.NotFound("Director", command.Id));

            _repository.Delete(director);
            await _unitOfWork.Commit(cancellationToken);

            return Result<bool>.AsSuccess(true);
        }
    }
}
