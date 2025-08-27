﻿using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Movie
{
    public record class DeleteMovieCommand(Guid Id) : ICommand<Result<bool>>;
}
