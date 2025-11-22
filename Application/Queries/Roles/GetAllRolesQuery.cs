using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Queries.Roles
{
    public record GetAllRolesQuery() : IQuery<Result<IEnumerable<RoleResponse>>>;
}
