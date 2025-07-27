using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Buisness.Helpers.Common.HelperEnums;
using Core.Enums;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.Logout
{
    public class LogoutCommandHandler : AuthCommandHandlerBase<LogoutCommand>, ICommandHandler<LogoutCommand, BaseResponse<bool>>
    {
        public LogoutCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogoutCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "Logout")
        {
        }

        public async Task<BaseResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName, request.AccessToken));

                var httpContext = _httpContextAccessor.HttpContext;

                LogoutRequestDto logoutRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, logoutRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(logoutRequestDto.AccessToken),

                        // Executors
                        ctx => _authBusinessLogicHelper.BlacklistSessionsAsync(logoutRequestDto.AccessToken, BlacklistMode.Single)
                    }/*,
                    onError: async (ctx, failedResult) =>
                    {
                        // Hangi adımda hata aldıysan contextten gör.
                        if (ctx.StepIndex == 2)
                        {
                            // Mesela 3. adımda hata olursa cleanup yap
                            await _authBusinessLogicHelper.LogRecoveryTokenIssueAsync();
                        }
                    }*/);

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.LogoutFailed,
                        methodName: nameof(LogoutCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogDebug(message: CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<bool>.Failure(
                            message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                            statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.LogoutSucceeded,
                    methodName: nameof(LogoutCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: logoutRequestDto.AccessToken,
                    isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName, request.AccessToken));
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
