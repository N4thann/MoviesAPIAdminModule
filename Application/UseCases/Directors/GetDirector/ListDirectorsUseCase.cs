using Application.Common;
using Application.DTOs.Mappings;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public  class ListDirectorsUseCase : IQueryHandler<ListDirectorsQuery, PagedList<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;

        public ListDirectorsUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<PagedList<DirectorInfoResponse>> Handle(ListDirectorsQuery query, CancellationToken cancellationToken)
        {
            var directors =  _repository.GetAllQueryable().OrderBy(p => p.Name);

            var directorsPaged = PagedList<Director>.ToPagedList(directors, 
                query.Parameters.PageNumber, query.Parameters.PageSize);

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
