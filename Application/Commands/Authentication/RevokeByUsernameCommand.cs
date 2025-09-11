using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class RevokeByUsernameCommand(string Username) : ICommand<Result<bool>>;
}
