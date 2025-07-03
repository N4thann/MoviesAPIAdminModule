using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public  class ListDirectorsUseCase : IQueryHandler<ListDirectorsQuery, IEnumerable<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;

        public ListDirectorsUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<IEnumerable<DirectorInfoResponse>> Handle(ListDirectorsQuery query, CancellationToken cancellationToken)
        {
            var directors = await _repository.GetAllAsync();

            var response = directors.Select(director => new DirectorInfoResponse(
                director.Id,
                director.Name,
                director.BirthDate,
                director.Country.Name,
                director.Country.Code,
                director.Biography,
                director.IsActive,
                director.CreatedAt,
                director.UpdatedAt,
                director.Age
                )).ToList();

                return response;
        }
    }
}
