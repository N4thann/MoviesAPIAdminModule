using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Queries.Director
{
    public record class GetDirectorByIdQuery(
        Guid Id
        ) : IQuery<Result<DirectorInfoResponse>>;
}
