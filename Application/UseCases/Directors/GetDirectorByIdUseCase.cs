using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Director;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Directors
{
    public class GetDirectorByIdUseCase : IQueryHandler<GetDirectorByIdQuery, Result<DirectorInfoResponse>>
    {
        private readonly IRepository<Director> _repository;

        public GetDirectorByIdUseCase(IRepository<Director> repository) => _repository = repository;

        public async Task<Result<DirectorInfoResponse>> Handle(GetDirectorByIdQuery query, CancellationToken cancellationToken)
        {
            var director = await _repository.GetByIdAsync(query.Id);

            if (director == null)
                return Result<DirectorInfoResponse>.AsFailure(Failure.NotFound("Director", query.Id));

            return director.ToDirectorDTO();
        }
    }
}
