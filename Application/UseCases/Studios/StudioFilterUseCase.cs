using Application.Common;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Studio;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios
{
    public class StudioFilterUseCase : IQueryHandler<StudioFilterQuery, PagedList<StudioInfoResponse>>
    {
        private readonly IRepository<Studio> _repository;

        public StudioFilterUseCase(IRepository<Studio> repository) => _repository = repository;

        public async Task<PagedList<StudioInfoResponse>> Handle(StudioFilterQuery query, CancellationToken cancellationToken)
        {
            var studios = _repository.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
                studios = studios.Where(s => s.Name.Contains(query.Name));
            if (!string.IsNullOrWhiteSpace(query.CountryName))
                studios = studios.Where(s => s.Country.Name.Contains(query.CountryName));
            if (query.FoundationYearBegin.HasValue)
                studios = studios.Where(s => s.FoundationDate.Year >= query.FoundationYearBegin.Value);
            if (query.FoundationYearEnd.HasValue)
                studios = studios.Where(s => s.FoundationDate.Year <= query.FoundationYearEnd.Value);

            studios = studios.Where(s => s.IsActive == query.Active);

            studios = studios.OrderBy(s => s.Name);

            var studiosPaged = PagedList<Studio>.ToPagedList(
                studios,
                query.Parameters.PageNumber,
                query.Parameters.PageSize
            );

            var response = studiosPaged.ToStudioPagedListDTO();

            return response;
        }
    }
}
