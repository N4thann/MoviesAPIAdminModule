using Application.Commands.Authentication;
using Application.Interfaces;
using Application.UseCases.Authentication;
using Bogus;
using Domain.Enums;
using Domain.Identity;
using Domain.SeedWork.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Tests.UseCases.Authentication
{
    public class LoginUseCaseTests
    {
        // --- SUT (System Under Test) ---
        private readonly LoginUseCase _sut;

        private readonly UserManager<ApplicationUser> _subUserManager;
        private readonly RoleManager<IdentityRole> _subRoleManager;
        private readonly ITokenService _subTokenService;
        private readonly IConfiguration _subConfiguration;

        private readonly Faker _faker;
        private readonly ApplicationUser _validUser;
        private readonly LoginCommand _loginCommand;

        #region CONSTRUCTOR
        public LoginUseCaseTests()
        {
            _faker = new Faker("pt_BR");

            var subUserStore = Substitute.For<IUserStore<ApplicationUser>>();
            var subRoleStore = Substitute.For<IRoleStore<IdentityRole>>();

            _subUserManager = Substitute.For<UserManager<ApplicationUser>>(subUserStore, null, null, null, null, null, null, null, null);
            _subRoleManager = Substitute.For<RoleManager<IdentityRole>>(subRoleStore, null, null, null, null);
            _subTokenService = Substitute.For<ITokenService>();
            _subConfiguration = Substitute.For<IConfiguration>();

            _loginCommand = new LoginCommand(
                UserName: _faker.Random.String2(8, "abcdefghijklmnopqrstuvwxyz"),
                Password: $"P@ss{_faker.Random.Number(99)}w{_faker.Lorem.Letter()}!"
            );

            _validUser = new ApplicationUser
            {
                UserName = _loginCommand.UserName,
                Email = _faker.Internet.Email(),
                RefreshToken = _faker.Random.AlphaNumeric(20),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            _sut = new LoginUseCase(
                _subUserManager,
                _subRoleManager,
                _subTokenService,
                _subConfiguration
            );
        }
        #endregion

        #region HAPPY PATCH
        [Fact]
        public async Task Handle_WhenCredentialsAreValid_ShouldReturnSuccessTokenResponse()
        {
            // Arrange
            var userRoles = new List<string> { "Admin" };
            var fakeRefreshToken = _faker.Random.AlphaNumeric(32);
            var fakeExpiration = DateTime.UtcNow.AddHours(1);
            var fakeJwtToken = new JwtSecurityToken(
                issuer: "test.com",
                audience: "test.com",
                expires: fakeExpiration
            );

            _subUserManager.FindByNameAsync(_loginCommand.UserName)
                           .Returns(Task.FromResult(_validUser));
            _subUserManager.CheckPasswordAsync(_validUser, _loginCommand.Password)
                           .Returns(Task.FromResult(true));
            _subUserManager.GetRolesAsync(_validUser)
                           .Returns(Task.FromResult<IList<string>>(userRoles));
            _subConfiguration["JWT:RefreshTokenValidityInMinutes"].Returns("60");
            _subTokenService.GenerateAccessToken(Arg.Any<List<Claim>>(), _subConfiguration)
                            .Returns(fakeJwtToken);
            _subTokenService.GenerateRefreshToken().Returns(fakeRefreshToken);
            _subUserManager.UpdateAsync(_validUser)
                           .Returns(Task.FromResult(IdentityResult.Success));

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.RefreshToken.Should().Be(fakeRefreshToken);
            _validUser.RefreshToken.Should().Be(fakeRefreshToken);
            await _subUserManager.Received(1).UpdateAsync(_validUser);

            result.Success.Expiration.Should().BeCloseTo(fakeExpiration, precision: TimeSpan.FromSeconds(1));
        }
        #endregion

        #region UNHappy PATH
        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldReturnInvalidCredentialsFailure()
        {
            // Arrange 
            _subUserManager.FindByNameAsync(_loginCommand.UserName)
                           .Returns(Task.FromResult<ApplicationUser?>(null));

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().Be(Failure.InvalidCredentials);

            //Testa se o comportamento (o "caminho" do código) foi o correto.
            await _subUserManager.DidNotReceive().CheckPasswordAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
            //DidNotReceive com Arg.Any prova que o seu código parou de executar exatamente onde deveria.
        }

        [Fact]
        public async Task Handle_WhenPasswordIsIncorrect_ShouldReturnInvalidCredentialsFailure()
        {
            // Arrange
            _subUserManager.FindByNameAsync(_loginCommand.UserName)
                           .Returns(Task.FromResult(_validUser));
            _subUserManager.CheckPasswordAsync(_validUser, _loginCommand.Password)
                           .Returns(Task.FromResult(false));

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().Be(Failure.InvalidCredentials);
            _subTokenService.DidNotReceive().GenerateAccessToken(Arg.Any<List<Claim>>(), Arg.Any<IConfiguration>());
        }

        [Fact]
        public async Task Handle_WhenUpdateAsyncFails_ShouldReturnInfrastructureFailure()
        {
            // Arrange
            var userRoles = new List<string> { "Admin" };
            var fakeRefreshToken = _faker.Random.AlphaNumeric(32);
            var fakeExpiration = DateTime.UtcNow.AddHours(1);
            var fakeJwtToken = new JwtSecurityToken(expires: fakeExpiration);
            var updateError = new IdentityError { Description = "Database error" };

            // Mock de sucesso até a falha
            _subUserManager.FindByNameAsync(_loginCommand.UserName).Returns(Task.FromResult(_validUser));
            _subUserManager.CheckPasswordAsync(_validUser, _loginCommand.Password).Returns(Task.FromResult(true));
            _subUserManager.GetRolesAsync(_validUser).Returns(Task.FromResult<IList<string>>(userRoles));
            _subConfiguration["JWT:RefreshTokenValidityInMinutes"].Returns("60");
            _subTokenService.GenerateAccessToken(Arg.Any<List<Claim>>(), _subConfiguration).Returns(fakeJwtToken);
            _subTokenService.GenerateRefreshToken().Returns(fakeRefreshToken);

            _subUserManager.UpdateAsync(_validUser)
                           .Returns(Task.FromResult(IdentityResult.Failed(updateError)));

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.Failure.Type.Should().Be(FailureType.Infrastructure);
            result.Failure.Message.Should().Contain("Failed to save refresh token");
            result.Failure.Message.Should().Contain(updateError.Description);
        }

        [Fact]
        public async Task Handle_WhenRefreshTokenConfigIsMissing_ShouldSetExpiryToImmediate()
        {
            // Arrange
            var userRoles = new List<string> { "Admin" };
            var fakeRefreshToken = _faker.Random.AlphaNumeric(32);
            var fakeExpiration = DateTime.UtcNow.AddHours(1);
            var fakeJwtToken = new JwtSecurityToken(expires: fakeExpiration);

            _subUserManager.FindByNameAsync(_loginCommand.UserName).Returns(Task.FromResult(_validUser));
            _subUserManager.CheckPasswordAsync(_validUser, _loginCommand.Password).Returns(Task.FromResult(true));
            _subUserManager.GetRolesAsync(_validUser).Returns(Task.FromResult<IList<string>>(userRoles));

            _subConfiguration["JWT:RefreshTokenValidityInMinutes"].Returns((string?)null);

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Type.Should().Be(FailureType.Infrastructure);
            result.Failure.Message.Should().Contain("missing or invalid");
        }

        [Fact]
        public async Task Handle_WhenUserHasNoRoles_ShouldReturnForbiddenFailure() 
        {
            // Arrange
            var emptyUserRoles = new List<string>();

            _subUserManager.FindByNameAsync(_loginCommand.UserName).Returns(Task.FromResult(_validUser));
            _subUserManager.CheckPasswordAsync(_validUser, _loginCommand.Password).Returns(Task.FromResult(true));

            _subUserManager.GetRolesAsync(_validUser).Returns(Task.FromResult<IList<string>>(emptyUserRoles));

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Type.Should().Be(FailureType.Forbidden);
            result.Failure.Message.Should().Contain("required role");
        }

        [Fact]
        public async Task Handle_WhenUserHasWrongRole_ShouldReturnForbiddenFailure()
        {
            // Arrange
            var wrongUserRoles = new List<string> { "User" };

            _subUserManager.FindByNameAsync(_loginCommand.UserName).Returns(Task.FromResult(_validUser));
            _subUserManager.CheckPasswordAsync(_validUser, _loginCommand.Password).Returns(Task.FromResult(true));

            _subUserManager.GetRolesAsync(_validUser).Returns(Task.FromResult<IList<string>>(wrongUserRoles));

            // Act
            var result = await _sut.Handle(_loginCommand, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Type.Should().Be(FailureType.Forbidden);
            result.Failure.Message.Should().Contain("required role");
        }

        #endregion
    }
}
