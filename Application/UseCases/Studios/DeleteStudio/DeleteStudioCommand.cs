using Application.Interfaces;

namespace Application.UseCases.Studios.DeleteStudio
{
    public record class DeleteStudioCommand(
    Guid Id
        ) : ICommand;
}
