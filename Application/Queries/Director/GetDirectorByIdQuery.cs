using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Queries.Director
{
    public record class GetDirectorByIdQuery(
        Guid Id
        ) : IQuery<Result<DirectorInfoResponse>>;
}
