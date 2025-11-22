using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Commands.Studio
{
    public record class DeleteStudioCommand(Guid Id) : ICommand<Result<bool>>;
}
 