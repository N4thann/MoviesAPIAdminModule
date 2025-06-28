using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.SeedWork.Interfaces;
using System.Data;

namespace Application.UseCases.Studios.UpdateStudio
{
    public class UpdateBasicInfoStudioUseCase : ICommandHandler<UpdateBasicInfoStudioCommand,StudioInfoResponse>
    {
        private readonly IStudioRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBasicInfoStudioUseCase(IStudioRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudioInfoResponse> Handle(UpdateBasicInfoStudioCommand command, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(command.Id);

            if (studio == null)
                throw new KeyNotFoundException($"Studio with ID {command.Id} not found.");
            try
            {
                studio.UpdateBasicInfo(command.Name, command.History); //não precisa salvar no contexto esse objeto studio?
                //testar se enviar vazio o campo history
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
