using Application.Commands.Authentication;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Authentication
{
    public class CreateRoleUseCase : ICommandHandler<CreateRoleCommand, Result<bool>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateRoleUseCase(RoleManager<IdentityRole> roleManager) => _roleManager = roleManager;

        public async Task<Result<bool>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            if (await _roleManager.RoleExistsAsync(command.RoleName))
                return Result<bool>.AsFailure(Failure.Conflict($"Role '{command.RoleName}' already exists."));

            var identityResult = await _roleManager.CreateAsync(new IdentityRole(command.RoleName));

            if (!identityResult.Succeeded)
            {
                var errors = string.Join("\n", identityResult.Errors.Select(e => e.Description));
                return Result<bool>.AsFailure(Failure.Validation(errors));
            }

            return Result<bool>.AsSuccess(true);
        }
    }
}
