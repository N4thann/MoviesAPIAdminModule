using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Studios.CreateStudio
{
    public class CreateStudioUseCase : ICommandHandler<CreateStudioCommand, StudioInfoResponse>
    {
        private readonly IStudioRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudioUseCase(IStudioRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudioInfoResponse> Handle(CreateStudioCommand command, CancellationToken cancellationToken)
        {
            var country = new Country(command.CountryName, command.CountryCode);

            var studio = new Studio(
                command.Name,
                country,
                command.FoundationDate,
                command.History
                ); 

            await _repository.AddAsync( studio );
            await _unitOfWork.Commit(cancellationToken);

            var response = new StudioInfoResponse(
                studio.Id,
                studio.Name,
                studio.Country.Name,
                studio.Country.Code,
                studio.FoundationDate,
                studio.History,
                studio.IsActive,
                studio.CreatedAt,
                studio.UpdatedAt,
                studio.YearsInOperation
                );

            return response;
        }

    }
}
