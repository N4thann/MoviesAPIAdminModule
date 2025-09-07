using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class LoginCommand(
        string UserName,
        string Password
        ) : ICommand<Result<LoginResponse>>;
}
