using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public class ActivateStudioUseCase : ICommandHandler<ActivateStudioCommand>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateStudioUseCase(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
                throw new KeyNotFoundException($"Studio with ID {command.Id} not found.");

            try
            {
                studio.Activate();
                await _unitOfWork.Commit(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Studio with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
