using Application.Commands.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors
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

            _repository.Delete(director);
            await _unitOfWork.Commit(cancellationToken);      
        }
    }
}
