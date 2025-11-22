using Application.Interfaces.Mediator;
using Domain.SeedWork.Core;

namespace Application.Commands.Movie
{
    public record class AddAwardCommand(
        Guid Id,
        int CategoryId,   
        int InstitutionId, 
        int Year
    ) : ICommand<Result<bool>>;
}
