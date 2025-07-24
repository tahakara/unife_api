using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword
{
    public class ForgotPasswordCommandHandler : AuthCommandHandlerBase<ForgotPasswordCommand>, ICommandHandler<ForgotPasswordCommand, BaseResponse<bool>>
    {
        public ForgotPasswordCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ForgotPasswordCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("ForgotPassword Strted.");

                var httpContext = _httpContextAccessor.HttpContext;

                ForgotPasswordRequestDto forgotPasswordRequestDto = new();

                IBuisnessLogicResult buisnessLogicResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, forgotPasswordRequestDto),
                        ctx => _authBusinessLogicHelper.CheckForgotPasswordCredentialsAsync(forgotPasswordRequestDto),
                        ctx => _authBusinessLogicHelper.PreventForgotBruteForceAsync(forgotPasswordRequestDto),
                        ctx => _authBusinessLogicHelper.SendRecoveryNotificaitonAsync(forgotPasswordRequestDto)
                    });

                if (buisnessLogicResult != null)
                {   _logger.LogError("Business logic validation failed: {Message}", buisnessLogicResult.Message);
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext,
                        string.Empty,
                        SecurityEventTypeGuid.PasswordResetRequest,
                        nameof(ForgotPasswordCommandHandler),
                        "Forgot Password",
                        false,
                        buisnessLogicResult.Message ?? "An error occurred while processing your request."
                    );
                    _logger.LogDebug("ForgotPassword Failed.");
                    return BaseResponse<bool>.Failure(
                        message: buisnessLogicResult.Message ?? "An error occurred while processing your request.",
                        statusCode: buisnessLogicResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext,
                    string.Empty,
                    SecurityEventTypeGuid.PasswordResetRequest,
                    nameof(ForgotPasswordCommandHandler),
                    "Forgot Password",
                    true
                );
                _logger.LogDebug("ForgotPassword Completed.");
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "Forgot password request processed successfully.",
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing ForgotPasswordCommand");
                return BaseResponse<bool>.Failure(
                    message: "An error occurred while processing your request.",
                    statusCode: 500);
            }
        }
    }
}
