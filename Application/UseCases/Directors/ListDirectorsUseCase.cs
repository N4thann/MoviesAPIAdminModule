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
    public  class ListDirectorsUseCase : IQueryHandler<ListDirectorsQuery, Result<IPagedList<DirectorInfoResponse>>>
    {
        private readonly IRepository<Director> _repository;

        public ListDirectorsUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<Result<IPagedList<DirectorInfoResponse>>> Handle(ListDirectorsQuery query, CancellationToken cancellationToken)
        {
            var directors =  _repository.GetAllQueryable().OrderBy(p => p.Name);

            var directorsPaged = await directors.ToPagedListAsync(query.Parameters.PageNumber, query.Parameters.PageSize, cancellationToken);

            return directorsPaged.ToDirectorPagedListDTO();
        }
    }
}
