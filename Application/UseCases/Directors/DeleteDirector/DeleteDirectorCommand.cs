using Application.Interfaces;

namespace Application.UseCases.Directors.DeleteDirector
{
    public record class DeleteDirectorCommand(
        Guid Id
        ) : ICommand;
}
