using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class AddUserToRoleCommand(
        string Email,
        string RoleName
        ) : ICommand<Result<bool>>;
}
