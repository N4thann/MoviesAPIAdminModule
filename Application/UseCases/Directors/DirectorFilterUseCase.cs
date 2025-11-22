using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Application.Queries.Director;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace Application.UseCases.Directors
{
    public class DirectorFilterUseCase : IQueryHandler<DirectorFilterQuery, Result<IPagedList<DirectorInfoResponse>>>
    {
        private readonly IRepository<Director> _repository;

        public DirectorFilterUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<Result<IPagedList<DirectorInfoResponse>>> Handle(DirectorFilterQuery query, CancellationToken cancellationToken)
        {
            var directors = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(query.Name))
                directors = directors.Where(d => d.Name.Contains(query.Name));
            if (!string.IsNullOrEmpty(query.CountryName))
                directors = directors.Where(d => d.Country.Name.Contains(query.CountryName));
            if (query.AgeBegin.HasValue)
            {
                var minBirthDateForAge = DateTime.Today.AddYears(-query.AgeBegin.Value);
                directors = directors.Where(d => d.BirthDate <= minBirthDateForAge);
            }
            if (query.AgeEnd.HasValue)
            {
                var maxBirthDateForAge = DateTime.Today.AddYears(-(query.AgeEnd.Value + 1)); 
                directors = directors.Where(d => d.BirthDate >= maxBirthDateForAge);
            }

            directors = directors.Where(d => d.IsActive == query.Active);

            directors = directors.OrderBy(d => d.Name);

            var directorsPaged = await directors.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            return directorsPaged.ToDirectorPagedListDTO();
        }
    }
}
