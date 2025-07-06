using Application.DTOs.Mappings;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Studios.CreateStudio
{
    public class CreateStudioUseCase : ICommandHandler<CreateStudioCommand, StudioInfoResponse>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudioUseCase(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudioInfoResponse> Handle(CreateStudioCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var country = new Country(command.CountryName, command.CountryCode);

                var studio = new Studio(
                    command.Name,
                    country,
                    command.FoundationDate,
                    command.History
                    );

                await _repository.AddAsync(studio);
                await _unitOfWork.Commit(cancellationToken);

                var response = studio.ToStudioDTO();

                return response;
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException($"An unexpected error occurred while creating Studio. Details: {ex.Message}", ex);
            }         
        }

    }
}
