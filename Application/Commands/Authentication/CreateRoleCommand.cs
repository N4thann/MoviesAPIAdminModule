using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class CreateRoleCommand(string RoleName) : ICommand<Result<bool>>;
}
