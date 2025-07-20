using Application.DTOs.Response;
using Application.Interfaces;

namespace Application.Queries.Director
{
    public record class GetDirectorByIdQuery(
        Guid Id
        ) : IQuery<DirectorInfoResponse>;
}
