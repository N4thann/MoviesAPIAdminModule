using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Directors.UpdateDirector
{
    public class DeactivateDirectorUseCase : ICommandHandler<DeactivateDirectorCommand>
    {
        private readonly IRepository<Director> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateDirectorUseCase(IRepository<Director> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeactivateDirectorCommand command, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(command.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {command.Id} not found.");

            try
            {
                director.Deactivate();
                await _unitOfWork.Commit(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Director with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
