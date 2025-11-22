using Application.Commands.Authentication;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Mediator;
using Domain.Identity;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Application.UseCases.Authentication
{
    public class RefreshTokenUseCase : ICommandHandler<RefreshTokenCommand, Result<TokenResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        public RefreshTokenUseCase(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    ITokenService tokenService,
                                    IConfiguration configuration) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(command.AccessToken, _configuration);

            if (principal?.Identity?.Name is null)
                return Result<TokenResponse>.AsFailure(Failure.Unauthorized("Invalid access token."));

            string userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName!);

            if (user is null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Result<TokenResponse>.AsFailure(Failure.Unauthorized("Invalid refresh token or session."));

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            var response = new TokenResponse(           
                new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                newRefreshToken,
                newAccessToken.ValidTo
             );

            return Result<TokenResponse>.AsSuccess(response);
        }
    }
}
