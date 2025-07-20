using Application.Common;
using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Director;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using X.PagedList;

namespace Application.UseCases.Directors
{
    public class DirectorFilterUseCase : IQueryHandler<DirectorFilterQuery, IPagedList<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;

        public DirectorFilterUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<IPagedList<DirectorInfoResponse>> Handle(DirectorFilterQuery query, CancellationToken cancellationToken)
        {
            var directors = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(query.Name))
                directors = directors.Where(d => d.Name.Contains(query.Name));//Não precise de StringComparison.OrdinalIgnoreCase nem nada do tipo, o banco de dados já aborda case-insensitive
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

            //Implementação antiga antes de implementar o pacote X.PagedList para tornar o método de paginação assíncrona
            //var directorsPaged = PagedList<Director>.ToPagedList(
            //    directors,
            //    query.Parameters.PageNumber,
            //    query.Parameters.PageSize
            //    );

            var directorsPaged = await directors.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize);

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
