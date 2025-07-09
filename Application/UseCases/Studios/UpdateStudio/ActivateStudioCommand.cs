using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class ActivateStudioCommand(
        Guid Id
        ) : ICommand;
}
