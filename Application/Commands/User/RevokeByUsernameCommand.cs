using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.User
{
    public record class RevokeByUsernameCommand(string Username) : ICommand<Result<bool>>;
}
