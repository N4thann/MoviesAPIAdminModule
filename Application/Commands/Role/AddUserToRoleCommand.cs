using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Commands.Role
{
    public record class AddUserToRoleCommand(
        string Email,
        string RoleName
        ) : ICommand<Result<bool>>;
}
