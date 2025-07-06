using Application.DTOs.Mappings;
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

            var response = directors.Select(director => director.ToDirectorDTO()); // IEnumberable, execução adiada
            return response; // Serializador percorrerá 'response' e executará ToDirectorDTO para cada item
        }
    }
}
