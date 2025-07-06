using Application.DTOs.Mappings;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Studios.UpdateStudio
{
    public class UpdateFoundationStudioUseCase : ICommandHandler<UpdateFoundationStudioCommand, StudioInfoResponse>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFoundationStudioUseCase(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudioInfoResponse> Handle(UpdateFoundationStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
                throw new KeyNotFoundException($"Studio with ID {command.Id} not found.");
            try
            {
                studio.UpdateFoundationDate(command.FoundationDate);
                await _unitOfWork.Commit(cancellationToken);

                var response = studio.ToStudioDTO();

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Studio with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
