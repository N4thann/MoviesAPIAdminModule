using Application.Interfaces;

namespace Application.Commands.Studio
{
    public record class DeleteStudioCommand(
    Guid Id
        ) : ICommand;
}
