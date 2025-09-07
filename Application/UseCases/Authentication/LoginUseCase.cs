using Application.Commands.Authentication;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.UseCases.Authentication
{
    public class LoginUseCase : ICommandHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginUseCase(
            IUserRepository userRepository,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByNameAsync(command.UserName);

            if (user is null || !await _userRepository.CheckPasswordAsync(user, command.Password))
            {
                return Result<LoginResponse>.AsFailure(Failure.InvalidCredentials);
            }

            var userRoles = await _userRepository.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("uid", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var accessToken = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var expiryMinutes = _configuration.GetValue<int>("JWT:RefreshTokenValidityInMinutes");
            var result = await _userRepository.UpdateUserRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMinutes(expiryMinutes));

            if (result.IsFailure)
                return Result<LoginResponse>.AsFailure(result.Failure!);

            var response = new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                Expiration = accessToken.ValidTo
            };

            return Result<LoginResponse>.AsSuccess(response);
        }
    }
}
