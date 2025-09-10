using Application.Commands.Authentication;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Identity;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.Authentication
{
    public class RegisterUseCase : ICommandHandler<RegisterCommand, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; //Caso tenha alguma implementação futura

        public RegisterUseCase(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<bool>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByNameAsync(command.UserName!);

            var userByUserName = await _userManager.FindByNameAsync(command.UserName);

            if (userByUserName != null)
                return Result<bool>.AsFailure(Failure.Conflict($"Username '{command.UserName}' is already taken."));

            var userByEmail = await _userManager.FindByEmailAsync(command.Email);

            if (userByEmail != null)
                return Result<bool>.AsFailure(Failure.Conflict($"Email '{command.Email}' is already in use."));

            ApplicationUser user = new()
            {
                Email = command.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = command.UserName
            };

            var identityResult = await _userManager.CreateAsync(user, command.Password);

            // Trata erros de validação do Identity (ex: senha fraca) como um erro 400
            if (!identityResult.Succeeded)
            {
                // Concatena todos os erros de validação em uma única mensagem
                var errors = string.Join("\n", identityResult.Errors.Select(e => e.Description));
                return Result<bool>.AsFailure(Failure.Validation(errors));
            }

            return Result<bool>.AsSuccess(true);
        }
    }
}
