using Application.Interfaces;

namespace Application.UseCases.Directors.UpdateDirector
{
    public record class DeactivateDirectorCommand(
        Guid Id
        ) : ICommand;
}
