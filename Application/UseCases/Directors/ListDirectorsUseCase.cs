using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Director;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Pandorax.PagedList;
using Pandorax.PagedList.EntityFrameworkCore;

namespace Application.UseCases.Directors
{
    public  class ListDirectorsUseCase : IQueryHandler<ListDirectorsQuery, IPagedList<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;

        public ListDirectorsUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<IPagedList<DirectorInfoResponse>> Handle(ListDirectorsQuery query, CancellationToken cancellationToken)
        {
            var directors =  _repository.GetAllQueryable().OrderBy(p => p.Name);

            //Implementação antiga antes de implementar o pacote X.PagedList para tornar o método de paginação assíncrona
            //var directorsPaged = PagedList<Director>.ToPagedList(directors, 
            //    query.Parameters.PageNumber, query.Parameters.PageSize);

            var directorsPaged = await directors.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            try
            {
                var responsePagedDto = directorsPaged.ToDirectorPagedListDTO();

                return responsePagedDto;
            }
            catch(InvalidOperationException)
            {
                throw;
            }
             
        }
    }
}
