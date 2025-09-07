using Application.DTOs.Authentication;
using Application.Interfaces;
using Domain.SeedWork.Core;
using Infraestructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infraestructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        public async Task<UserDto?> FindByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<bool> CheckPasswordAsync(UserDto userDto, string password)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null) return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetRolesAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            if (user == null) return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<Result<bool>> UpdateUserRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.AsFailure(Failure.NotFound("User", userId));
            }

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded
                ? Result<bool>.AsSuccess(true)
                : Result<bool>.AsFailure(Failure.InfrastructureError("Failed to update user refresh token."));
        }
    }
}
