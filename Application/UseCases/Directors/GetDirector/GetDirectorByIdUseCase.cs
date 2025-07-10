using Application.DTOs.Mappings;
using Application.DTOs.Response.Director;
using Application.Interfaces;
using Domain.Entities;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public class GetDirectorByIdUseCase : IQueryHandler<GetDirectorByIdQuery, DirectorInfoResponse>
    {
        private readonly IRepository<Director> _repository;

        public GetDirectorByIdUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<DirectorInfoResponse> Handle(GetDirectorByIdQuery query, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(query.Id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {query.Id} not found.");

            var response = director.ToDirectorDTO();
            return response;
        }
    }
}
