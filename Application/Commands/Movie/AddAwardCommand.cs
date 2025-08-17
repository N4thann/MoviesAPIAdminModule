using Application.Interfaces;

namespace Application.Commands.Movie
{
    public record class AddAwardCommand(
        Guid Id,
        int CategoryId,   
        int InstitutionId, 
        int Year
    ) : ICommand;
}
