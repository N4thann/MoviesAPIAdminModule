using Application.DTOs.Response;
using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class LoginCommand(
        string UserName,
        string Password
        ) : ICommand<Result<TokenResponse>>;
}
