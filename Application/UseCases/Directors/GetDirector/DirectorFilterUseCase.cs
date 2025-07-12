using Application.Common;
using Application.DTOs.Mappings;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public class DirectorFilterUseCase : IQueryHandler<DirectorFilterQuery, PagedList<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;

        public DirectorFilterUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<PagedList<DirectorInfoResponse>> Handle(DirectorFilterQuery query, CancellationToken cancellationToken)
        {
            var directors = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(query.Name))
                directors = directors.Where(d => d.Name.Contains(query.Name));//Não precise de StringComparison.OrdinalIgnoreCase nem nada do tipo, o banco de dados já aborda case-insensitive
            if (!string.IsNullOrEmpty(query.CountryName))
                directors = directors.Where(d => d.Country.Name.Contains(query.CountryName));
            if (query.AgeBegin.HasValue)
                directors = directors.Where(d => d.Age >= query.AgeBegin.Value);
            if (query.AgeEnd.HasValue)
                directors = directors.Where(d => d.Age <= query.AgeBegin.Value);

            directors = directors.Where(d => d.IsActive == query.Active);

            directors = directors.OrderBy(d => d.Name);

            var directorsPaged = PagedList<Director>.ToPagedList(
                directors,
                query.Parameters.PageNumber,
                query.Parameters.PageSize
                );
            try
            {
                var response = directorsPaged.ToDirectorPagedListDTO();

                return response;
            }
            catch(InvalidOperationException)
            {
                throw;
            }
            
        }
    }
}
