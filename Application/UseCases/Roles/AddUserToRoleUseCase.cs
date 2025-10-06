using Application.Commands.Role;
using Application.Interfaces;
using Domain.Identity;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Roles
{
    public class AddUserToRoleUseCase : ICommandHandler<AddUserToRoleCommand, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddUserToRoleUseCase(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<bool>> Handle(AddUserToRoleCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user is null)
            {
                return Result<bool>.AsFailure(Failure.NotFound("User", command.Email));
            }

            if (!await _roleManager.RoleExistsAsync(command.RoleName))
            {
                return Result<bool>.AsFailure(Failure.NotFound("Role", command.RoleName));
            }

            if (await _userManager.IsInRoleAsync(user, command.RoleName))
            {
                return Result<bool>.AsFailure(Failure.Conflict($"User is already in role '{command.RoleName}'."));
            }

            var identityResult = await _userManager.AddToRoleAsync(user, command.RoleName);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join("\n", identityResult.Errors.Select(e => e.Description));
                return Result<bool>.AsFailure(Failure.Validation(errors));
            }

            return Result<bool>.AsSuccess(true);
        }
    }
}
