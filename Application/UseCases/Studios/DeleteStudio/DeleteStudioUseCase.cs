using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios.DeleteStudio
{
    public class DeleteStudioUseCase : ICommandHandler<DeleteStudioCommand>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStudioUseCase(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
                throw new KeyNotFoundException($"Director with ID {command.Id} not found.");
            try
            {
                _repository.Delete(studio);
                await _unitOfWork.Commit(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while delete studio with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
