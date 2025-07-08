using Application.Interfaces;

namespace Application.UseCases.Directors.UpdateDirector
{
    public record class ActivateDirectorCommand(
        Guid Id
        ) : ICommand;
}
