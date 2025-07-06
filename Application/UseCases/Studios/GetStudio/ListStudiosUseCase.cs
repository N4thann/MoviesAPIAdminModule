using Application.DTOs.Mappings;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public class ListStudiosUseCase : IQueryHandler<ListStudiosQuery, IEnumerable<StudioInfoResponse>>
    {
        private readonly IRepository<Studio> _repository;

        public ListStudiosUseCase(IRepository<Studio> repository) => _repository = repository;

        public async Task<IEnumerable<StudioInfoResponse>> Handle(ListStudiosQuery query, CancellationToken cancellationToken)
        {
            var studios = await _repository.GetAllAsync();

            var response = studios.Select(studio => studio.ToStudioDTO());

            return response;
        }
    }
}
