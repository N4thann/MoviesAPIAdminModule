using Application.Interfaces;

namespace Application.UseCases.Studios.UpdateStudio
{
    public record class DeactivateStudioCommand(
        Guid Id
        ) : ICommand;
}
