using Application.Interfaces;

namespace Application.Commands.Movie
{
    public record class DeleteMovieCommand(Guid Id) : ICommand;
}
