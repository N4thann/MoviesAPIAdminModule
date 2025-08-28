using Application.DTOs.Mappings;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Queries.Studio;
using Domain.Entities;
using Domain.SeedWork.Core;
using Domain.SeedWork.Interfaces;

namespace Application.UseCases.Studios
{
    public class GetStudioByIdUseCase : IQueryHandler<GetStudioByIdQuery, Result<StudioInfoResponse>>
    {
        private readonly IRepository<Studio> _repository;

        public GetStudioByIdUseCase(IRepository<Studio> repository)  => _repository = repository;

        public async Task<Result<StudioInfoResponse>> Handle(GetStudioByIdQuery query, CancellationToken cancellationToken)
        {
            var studio = await _repository.GetByIdAsync(query.Id);

            if (studio == null)
                return Result<StudioInfoResponse>.AsFailure(Failure.NotFound("Studio", query.Id));

            return studio.ToStudioDTO();
        }
    }
}
