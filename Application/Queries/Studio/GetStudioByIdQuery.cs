using Application.DTOs.Response.Studio;
using Application.Interfaces;

namespace Application.Queries.Studio
{
    public record class GetStudioByIdQuery(
        Guid Id
        ) : IQuery<StudioInfoResponse>;
}
