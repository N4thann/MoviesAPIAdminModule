using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Studios.UpdateStudio
{
    public class UpdateCountryStudioUseCase : ICommandHandler<UpdateCountryStudioCommand, StudioInfoResponse>
    {
        private readonly IRepository<Studio> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCountryStudioUseCase(IRepository<Studio> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudioInfoResponse> Handle(UpdateCountryStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
                throw new KeyNotFoundException($"Studio with ID {command.Id} not found.");
            try
            {
                var country = new Country(command.CountryName, command.CountryCode);
                studio.UpdateCountry(country);//Quando uma entidade é carregada do DbContext (que seu repositório está usando), o EF Core automaticamente começa a rastrear essa entidade.
                //Por isso não precisamos de um método de Update do Repositório
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
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while updating Studio with ID {command.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
