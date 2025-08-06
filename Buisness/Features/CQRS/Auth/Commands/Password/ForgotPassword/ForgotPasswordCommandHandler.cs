using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
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

namespace Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword
{
    public class ForgotPasswordCommandHandler : AuthCommandHandlerBase<ForgotPasswordCommand>, ICommandHandler<ForgotPasswordCommand, BaseResponse<bool>>
    {
        public ForgotPasswordCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ForgotPasswordCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "ForgotPassword")
        {
        }

        public async Task<BaseResponse<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(CQRSLogMessages.ProccessStarted(_commandFullName));

                var httpContext = _httpContextAccessor.HttpContext;

                ForgotPasswordRequestDto forgotPasswordRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, forgotPasswordRequestDto),
                        ctx => _authBusinessLogicHelper.CheckForgotPasswordCredentialsAsync(forgotPasswordRequestDto),
                        ctx => _authBusinessLogicHelper.PreventForgotBruteForceAsync(forgotPasswordRequestDto),
                        ctx => _authBusinessLogicHelper.SendRecoveryNotificationAsync(forgotPasswordRequestDto)
                    });

                if (buisnessResult != null)
                {
                    //await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    //    httpContext: httpContext,
                    //    eventTypeGuidKey: SecurityEventTypeGuid.PasswordResetRequestFailed,
                    //    methodName: nameof(ForgotPasswordCommandHandler),
                    //    description: _commandFullName,
                    //    userGuid: null,
                    //    userTypeId: UserTypeId._,
                    //    accessToken: null,
                    //    isEventSuccess: false,
                    //    failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogDebug(CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<bool>.Failure(
                        message: CQRSResponseMessages.Fail(_commandFullName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }


                //await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                //    httpContext: httpContext,
                //    eventTypeGuidKey: SecurityEventTypeGuid.PasswordResetRequestSucceeded,
                //    methodName: nameof(ForgotPasswordCommandHandler),
                //    description: _commandFullName,
                //    userGuid: null,
                //    userTypeId: UserTypeId._,
                //    accessToken: null,
                //    isEventSuccess: true);

                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName));
            }
            catch (Exception ex)
            {
                _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}
