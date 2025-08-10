using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Commands.Movie
{
    public record PatchMovieCommand(
        Guid Id,
        JsonPatchDocument<Domain.Entities.Movie> PatchDocument
        ) : ICommand<MovieInfoBasicResponse>;
}
