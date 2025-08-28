using Application.Commands.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios
{
    public class DeleteStudioUseCase : ICommandHandler<DeleteStudioCommand, Result<bool>>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudioUseCase(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
                return Result<bool>.AsFailure(Failure.NotFound("Studio", command.Id));

            _repository.Delete(studio);
            await _unitOfWork.Commit(cancellationToken);

            return Result<bool>.AsSuccess(true);
        }
    }
}
