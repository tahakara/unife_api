using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutOthers;
using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyPhone;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.Base;
using WebAPI.Filters;

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
        [RejectAuthorizationHeader]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUpPost(
            [FromBody] SignUpCommand command) 
        {
            return await SendCommand(command);
        }

        [HttpPost("signin")]
        [RejectAuthorizationHeader]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignInPost(
            [FromBody] SignInCommand command)
        {
            return await SendCommand(command);
        }

        [HttpPost("verify-otp")]
        [RejectAuthorizationHeader]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyOTPPost(
            [FromBody] VerifyOTPCommand command)
        {
            return await SendCommand(command);
        }

        [HttpPost("resend-otp")]
        [RejectAuthorizationHeader]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResendSignInOTPPost(
            [FromBody] ResendSignInOTPCommand command)
        {
            return await SendCommand(command);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshTokenPost(
            [FromHeader(Name = "Authorization")] string? accessToken, 
            [FromBody] RefreshTokenCommand command) 
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        [Authorize]
        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout(
            [FromHeader(Name = "Authorization")] string accessToken)
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
        public async Task<IActionResult> LogoutOthers(
            [FromHeader(Name = "Authorization")] string accessToken)
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
        public async Task<IActionResult> LogoutAll(
            [FromHeader(Name = "Authorization")] string accessToken)
        {
            return await SendCommand(new LogoutAllCommand
            {
                AccessToken = accessToken
            });
        }

        [RejectAuthorizationHeader]
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPasswordPost(
            [FromBody] ForgotPasswordCommand command)
        {
            return await SendCommand(command);
        }

        [RejectAuthorizationHeader]
        [HttpPost("forgot-password/t/{RecoveryToken}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPasswordWithTokenPost(
            [FromRoute] string RecoveryToken,
            [FromBody] ForgotPasswordRecoveryTokenCommand command)
        {
            command.RecoveryToken = RecoveryToken;
            return await SendCommand(command);
        }


        [Authorize]
        [HttpPost("cahnge-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePasswordPost(
            [FromHeader(Name = "Authorization")] string? accessToken, 
            [FromBody] ChangePasswordCommand command)
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);

        }



        [Authorize]
        [HttpGet("verify-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmailGet(
            [FromHeader(Name = "Authorization")] string? accessToken)
        {
            return await SendCommand(
                new SendEmailVerificationOTPCommand { AccessToken = accessToken });
        }

        [Authorize]
        [HttpPost("verify-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmailPost(
            [FromHeader(Name = "Authorization")] string? accessToken, 
            [FromBody] VerifyEmailCommand command)
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }

        [Authorize]
        [HttpGet("verify-phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyPhoneGet(
            [FromHeader(Name = "Authorization")] VerifyPhoneCommand command)
        {
            return await SendCommand(command);
        }

        [Authorize]
        [HttpPost("verify-phone")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyPhone(
            [FromHeader(Name = "Authorization")] string? accessToken, 
            [FromBody] VerifyPhoneCommand command)
        {
            command.AccessToken = accessToken;
            return await SendCommand(command);
        }
    }
}
