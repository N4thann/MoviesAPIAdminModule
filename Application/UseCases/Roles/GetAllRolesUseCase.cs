using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Application.Queries.Roles;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Roles
{
    public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, Result<IEnumerable<RoleResponse>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetAllRolesQueryHandler(RoleManager<IdentityRole> roleManager) => _roleManager = roleManager;

        public async Task<Result<IEnumerable<RoleResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleResponse(r.Id, r.Name))
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<RoleResponse>>.AsSuccess(roles);
        }
    }
}
