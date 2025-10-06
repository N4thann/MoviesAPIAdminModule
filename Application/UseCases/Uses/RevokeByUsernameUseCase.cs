using Application.Commands.User;
using Application.Interfaces;
using Domain.Identity;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Uses
{
    public class RevokeByUsernameUseCase : ICommandHandler<RevokeByUsernameCommand, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RevokeByUsernameUseCase(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        public async Task<Result<bool>> Handle(RevokeByUsernameCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.Username);

            if (user == null) return Result<bool>.AsFailure(Failure.InvalidCredentials);

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return Result<bool>.AsSuccess(true);
        }
    }
}
