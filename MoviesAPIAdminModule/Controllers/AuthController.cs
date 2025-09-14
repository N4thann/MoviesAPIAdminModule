using Application.Commands.Authentication;
using Application.DTOs.Authentication;
using Application.DTOs.Response;
using Application.Interfaces;
using Asp.Versioning;
using Domain.SeedWork.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIAdminModule.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace MoviesAPIAdminModule.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Autentica um usuário e retorna os tokens de acesso e atualização", Tags = new[] { "Authentication Commands" })]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.UserName!, request.Password!);

            var result = await _mediator.Send<LoginCommand, Result<TokenResponse>>(command, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Failure!);

            return Ok(result.Success);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Cria uma nova conta de usuário no sistema", Tags = new[] { "Authentication Commands" })]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(request.UserName!, request.Email!, request.Password!);

            var result = await _mediator.Send<RegisterCommand, Result<bool>>(command, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Failure!);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Emite um novo token de acesso utilizando um token de atualização válido", Tags = new[] { "Authentication Commands" })]
        public async Task<IActionResult> RefreshToken(TokenRequest request, CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);

            var result = await _mediator.Send<RefreshTokenCommand, Result<TokenResponse>>(command, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Failure!);

            return Ok(result.Success);
        }


        [HttpPost]
        [Route("{username}")]
        [Authorize(Policy = "ExclusiveOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "(Admin) Invalida a sessão de um usuário, forçando um novo login", Tags = new[] { "Authentication Commands" })]
        public async Task<IActionResult> Revoke(string username, CancellationToken cancellationToken)
        {
            var command = new RevokeByUsernameCommand(username);

            var result = await _mediator.Send<RevokeByUsernameCommand, Result<bool>>(command, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Failure!);

            return NoContent();
        }

        [HttpPost("create")]
        [Authorize(Policy = "SuperAdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "(Admin) Cria uma nova role (função) no sistema.", Tags = new[] { "Roles Commands" })]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateRoleCommand(request.RoleName);
            var result = await _mediator.Send<CreateRoleCommand, Result<bool>>(command, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Failure!);

            return NoContent();
        }

        [HttpPost("add-user-to-role")]
        [Authorize(Policy = "SuperAdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "(Admin) Adiciona um usuário a uma role existente.", Tags = new[] { "Roles Commands" })]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleRequest request, CancellationToken cancellationToken)
        {
            var command = new AddUserToRoleCommand(request.Email, request.RoleName);
            var result = await _mediator.Send<AddUserToRoleCommand, Result<bool>>(command, cancellationToken);

            if (result.IsFailure)
                return HandleFailure(result.Failure!);

            return NoContent();
        }
    }
}
