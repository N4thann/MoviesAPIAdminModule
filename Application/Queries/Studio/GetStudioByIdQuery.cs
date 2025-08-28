using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Queries.Studio
{
    public record class GetStudioByIdQuery(
        Guid Id
        ) : IQuery<Result<StudioInfoResponse>>;
}
