using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.DeleteDirector
{
    public class DeleteDirectorUseCase : ICommandHandler<DeleteDirectorCommand>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteDirectorCommand command, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(command.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {command.Id} not found.");
            try
            {
                _repository.Delete(director);
                await _unitOfWork.Commit(cancellationToken);
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException($"An unexpected error occurred while delete director with ID {command.Id}. Details: {ex.Message}", ex);
            }        
        }
    }
}
