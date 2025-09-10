using Application.Commands.Authentication;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Identity;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.UseCases.Authentication
{
    public class LoginUseCase : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginUseCase(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<Result<TokenResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, command.Password))
                return Result<TokenResponse>.AsFailure(Failure.InvalidCredentials);

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                                out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            var response = new TokenResponse(
                new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken,
                token.ValidTo
            );

            return Result<TokenResponse>.AsSuccess(response);
        }
    }
}
