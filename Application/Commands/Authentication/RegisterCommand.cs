using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class RegisterCommand(
        string UserName,
        string Email,
        string Password,
        string? PhoneNumber
        ) : ICommand<Result<bool>>;
}
