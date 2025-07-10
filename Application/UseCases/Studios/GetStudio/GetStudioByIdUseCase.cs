using Application.DTOs.Mappings;
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

            var response = studio.ToStudioDTO();

            return response;
        }
    }
}
