using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public class GetStudioByIdUseCase : IQueryHandler<GetStudioByIdQuery, StudioInfoResponse>
    {
        private readonly IRepository<Studio> _repository;

        public GetStudioByIdUseCase(IRepository<Studio> repository)  => _repository = repository;

        public async Task<StudioInfoResponse> Handle(GetStudioByIdQuery query, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(query.Id);

            if (studio == null)
                throw new KeyNotFoundException($"Studio with ID {query.Id} not found.");
            try
            {
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
                throw new InvalidOperationException($"An unexpected error occurred while updating Studio with ID {query.Id}. Details: {ex.Message}", ex);
            }
        }
    }
}
