using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos;
using Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth.Base;
using Core.Enums;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail
{
    public class VerifyEmailCommandHandler : AuthCommandHandlerBase<VerifyEmailCommand>, ICommandHandler<VerifyEmailCommand, BaseResponse<bool>>
    {
        public VerifyEmailCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VerifyEmailCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "VerifyEmail")
        {
        }

        public async Task<BaseResponse<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName));

                var httpContext = _httpContextAccessor.HttpContext;

                VerifyEmailRequestDto verifyEmailPostRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, verifyEmailPostRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(verifyEmailPostRequestDto.AccessToken),
                        ctx => _authBusinessLogicHelper.CheckAndVerifyEmailOTPAsync(verifyEmailPostRequestDto.AccessToken, verifyEmailPostRequestDto.OtpCode),

                        // Executors
                        ctx => _authBusinessLogicHelper.CompleteEmailVerificaitonAsync(verifyEmailPostRequestDto.AccessToken)

                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.VerificationEmailFailed,
                        methodName: nameof(ResendSignInOTPCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? CQRSResponseMessages.Error(_commandName),
                        statusCode: 400);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.VerificationEmailSucceeded,
                    methodName: nameof(VerifyOTPCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: null,
                    isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
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
