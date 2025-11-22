using Application.Common;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Application.Queries.Studio;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace Application.UseCases.Studios
{
    public class StudioFilterUseCase : IQueryHandler<StudioFilterQuery, Result<IPagedList<StudioInfoResponse>>>
    {
        private readonly IRepository<Studio> _repository;

        public StudioFilterUseCase(IRepository<Studio> repository) => _repository = repository;

        public async Task<Result<IPagedList<StudioInfoResponse>>> Handle(StudioFilterQuery query, CancellationToken cancellationToken)
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

            var studiosPaged = await studios.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            return studiosPaged.ToStudioPagedListDTO();
        }
    }
}
