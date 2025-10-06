using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Role
{
    public record class CreateRoleCommand(string RoleName) : ICommand<Result<bool>>;
}
