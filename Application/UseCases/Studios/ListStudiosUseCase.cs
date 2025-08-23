using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Studio;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace Application.UseCases.Studios
{
    public class ListStudiosUseCase : IQueryHandler<ListStudiosQuery, IPagedList<StudioInfoResponse>>
    {
        private readonly IRepository<Studio> _repository;

        public ListStudiosUseCase(IRepository<Studio> repository) => _repository = repository;

        public async Task<IPagedList<StudioInfoResponse>> Handle(ListStudiosQuery query, CancellationToken cancellationToken)
        {
            var studios = _repository.GetAllQueryable().OrderBy(p => p.Name);

            var studiosPaged = await studios.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            var responsePagedDto = studiosPaged.ToStudioPagedListDTO();

            return responsePagedDto;
        }
    }
}
