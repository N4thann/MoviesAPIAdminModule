using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Queries.Roles
{
    public record GetAllRolesQuery() : IQuery<Result<IEnumerable<RoleResponse>>>;
}
