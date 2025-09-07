using Application.Commands.Authentication;
using Application.DTOs.Authentication;
using Application.DTOs.Response;
using Application.Interfaces;
using Asp.Versioning;
using Domain.SeedWork.Core;
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
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Realiza o login de um usuário", Tags = new[] { "Authentication Commands" })]
        public async Task<IActionResult> Login([FromBody] LoginModel request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.UserName!, request.Password!);

            var result = await _mediator.Send<LoginCommand, Result<LoginResponse>>(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Failure);
            }

            return Ok(result.Success);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Failure), StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(Summary = "Realiza o registro de um usuário", Tags = new[] { "Authentication Commands" })]
        public async Task<IActionResult> Register([FromBody] RegisterModel request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(request.UserName!,request.Email!, request.Password!);

            var result = await _mediator.Send<RegisterCommand, Result<RegisterResponse>>(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Failure);
            }

            return Ok(result.Success);
        }

    }
}
