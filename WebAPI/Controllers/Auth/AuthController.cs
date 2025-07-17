using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Auth.Commands.LogoutAll;
using Buisness.Features.CQRS.Auth.Commands.LogoutOthers;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator, ILogger<UniversityController> logger) 
            : base(mediator, logger)
        {
        }
        
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUp([FromHeader(Name = "Authorization")] string? accessToken, [FromBody] SignUpCommand command) 
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignIn([FromHeader(Name = "Authorization")] string? accessToken, [FromBody] SignInCommand command)
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "Authorization")] string? accessToken, [FromBody] RefreshTokenCommand command) 
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        [Authorize]
        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string accessToken)
        {
            return await SendCommand(new LogoutCommand
            {
                AccessToken = accessToken
            });
        }

        [Authorize]
        [HttpGet("logout-others")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogoutOthers([FromHeader(Name = "Authorization")] string accessToken)
        {
            return await SendCommand(new LogoutOthersCommand
            {
                AccessToken = accessToken
            });
        }

        [Authorize]
        [HttpGet("logout-all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogoutAll([FromHeader(Name = "Authorization")] string accessToken)
        {
            return await SendCommand(new LogoutAllCommand
            {
                AccessToken = accessToken
            });
        }
    }
}
