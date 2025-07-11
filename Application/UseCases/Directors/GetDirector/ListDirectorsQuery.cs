using Application.Common;
using Application.DTOs.Response.Director;
using Application.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public record class ListDirectorsQuery(
        DirectorParameters Parameters) : IQuery<PagedList<DirectorInfoResponse>>;
}
