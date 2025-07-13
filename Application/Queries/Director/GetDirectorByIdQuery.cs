using Application.DTOs.Response.Director;
using Application.Interfaces;

namespace Application.Queries.Director
{
    public record class GetDirectorByIdQuery(
        Guid Id
        ) : IQuery<DirectorInfoResponse>;
}
