﻿using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;

namespace Application.Commands.Authentication
{
    public record class RefreshTokenCommand(
        string AccessToken,
        string RefreshToken
        ) : ICommand<Result<TokenResponse>>;
}
