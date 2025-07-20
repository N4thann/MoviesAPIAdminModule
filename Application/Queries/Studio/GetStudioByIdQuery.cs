using Application.DTOs.Response;
using Application.Interfaces;

namespace Application.Queries.Studio
{
    public record class GetStudioByIdQuery(
        Guid Id
        ) : IQuery<StudioInfoResponse>;
}
