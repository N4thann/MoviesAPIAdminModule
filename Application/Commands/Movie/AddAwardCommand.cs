using Application.Interfaces;

namespace Application.Commands.Movie
{
    public record class AddAwardCommand(
        Guid Id,
        string Name,
        string Institution,
        int Year
    ) : ICommand;
}
