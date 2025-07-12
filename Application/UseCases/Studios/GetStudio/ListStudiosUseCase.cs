using Application.Common;
using Application.DTOs.Mappings;
using Application.DTOs.Response.Studio;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios.GetStudio
{
    public class ListStudiosUseCase : IQueryHandler<ListStudiosQuery, PagedList<StudioInfoResponse>>
    {
        private readonly IRepository<Studio> _repository;

        public ListStudiosUseCase(IRepository<Studio> repository) => _repository = repository;

        public async Task<PagedList<StudioInfoResponse>> Handle(ListStudiosQuery query, CancellationToken cancellationToken)
        {
            var studios = _repository.GetAllQueryable().OrderBy(p => p.Name);

            var studiosPaged = PagedList<Studio>.ToPagedList(studios,
                query.Parameters.PageNumber, query.Parameters.PageSize);

            var responsePagedDto = studiosPaged.ToStudioPagedListDTO();

            return responsePagedDto;
        }
    }
}
