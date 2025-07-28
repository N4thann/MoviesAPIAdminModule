using Application.Interfaces;

namespace Application.Commands.Director
{
    public record class DeleteDirectorCommand(Guid Id) : ICommand;
}
