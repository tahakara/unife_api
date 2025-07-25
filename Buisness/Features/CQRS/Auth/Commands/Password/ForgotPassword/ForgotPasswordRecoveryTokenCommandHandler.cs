using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword
{
    public class ForgotPasswordRecoveryTokenCommandHandler : AuthCommandHandlerBase<ForgotPasswordRecoveryTokenCommand>, ICommandHandler<ForgotPasswordRecoveryTokenCommand, BaseResponse<bool>>
    {
        public ForgotPasswordRecoveryTokenCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ForgotPasswordRecoveryTokenCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "ForgotPasswordRecoveryTokenCommand")
        {
        }

        public async Task<BaseResponse<bool>> Handle(ForgotPasswordRecoveryTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(CQRSLogMessages.ProccessStarted(_commandFullName, request.RecoveryToken));

                var httpContext = _httpContextAccessor.HttpContext;

                ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, forgotPasswordRecoveryTokenRequestDto),
                        ctx => _authBusinessLogicHelper.CheckRecoveryToken(forgotPasswordRecoveryTokenRequestDto),
                        ctx => _authBusinessLogicHelper.ResetUserPasswordAsync(forgotPasswordRecoveryTokenRequestDto) 
                    });

                if (buisnessResult != null)
                {
                    //await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    //    httpContext,
                    //    string.Empty,
                    //    SecurityEventTypeGuid.PasswordResetSuccess,
                    //    nameof(ForgotPasswordRecoveryTokenCommandHandler),
                    //    "Forgot Password Recovery Token",
                    //    false,
                    //    buisnessResult.Message ?? "An error occurred while processing your request.");
                    _logger.LogDebug(CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<bool>.Failure(
                        message : CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }

                //await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                //    httpContext,
                //    string.Empty,
                //    SecurityEventTypeGuid.PasswordResetSuccess,
                //    nameof(ForgotPasswordRecoveryTokenCommandHandler),
                //    "Forgot Password Recovery Token",
                //    true,
                //    buisnessResult.Message ?? "Recovery token processed successfully.");
                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName, request.RecoveryToken));
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    statusCode: 500);
            }
        }
    }
}
