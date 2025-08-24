using Application.Interfaces;
using Domain.SeedWork.Core;
using static Domain.ValueObjects.MovieImage;

namespace Application.Commands.Movie
{
    public record AddMovieImageCommand(
        Guid Id,
        Stream FileStream,
        string OriginalFileName,
        string ContentType,
        ImageType ImageType,
        string? AltText
    ) : ICommand<Result<string>>;
}
