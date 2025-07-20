using Application.Common;
using Application.Common.Parameters;
using Application.DTOs.Response;
using Application.Interfaces;
using X.PagedList;

namespace Application.Queries.Director
{
    public record class ListDirectorsQuery(
        DirectorParametersRequest Parameters) : IQuery<IPagedList<DirectorInfoResponse>>;
}
