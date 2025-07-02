using Application.DTOs.Response.Director;
using Application.Interfaces;

namespace Application.UseCases.Directors.GetDirector
{
    public record class ListDirectorsQuery : IQuery<IEnumerable<DirectorInfoResponse>>;
}
