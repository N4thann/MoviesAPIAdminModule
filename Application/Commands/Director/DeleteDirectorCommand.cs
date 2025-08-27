using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Director
{
    public record class DeleteDirectorCommand(Guid Id) : ICommand<Result<bool>>;
}
