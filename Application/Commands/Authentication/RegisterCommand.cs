using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class RegisterCommand(
        string UserName,
        string Email,
        string Password
        ) : ICommand<Result<bool>>;
}
