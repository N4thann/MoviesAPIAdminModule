using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.SeedWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Studios.GetStudio
{
    public class ListStudiosUseCase : IQueryHandler<ListStudiosQuery, IEnumerable<StudioInfoResponse>>
    {
        private readonly IStudioRepository _repository;

        public ListStudiosUseCase(IStudioRepository repository) => _repository = repository;

        public async Task<IEnumerable<StudioInfoResponse>> Handle(ListStudiosQuery command, CancellationToken cancellationToken)
        {
            var studios = await _repository.GetAllAsync();

            var response = studios.Select(studio => new StudioInfoResponse(
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
                )).ToList();

            return response;
        }
    }
}
