using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth.Base;
using Buisness.Helpers.Common.HelperEnums;
using Core.Enums;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.LogoutOthers
{
    public class LogoutOthersCommandHandler : AuthCommandHandlerBase<LogoutOthersCommand>, ICommandHandler<LogoutOthersCommand, BaseResponse<bool>>
    {
        public LogoutOthersCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogoutOthersCommand> logger) 
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "LogoutOthers")
        {
        }

        public async Task<BaseResponse<bool>> Handle(LogoutOthersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(CQRSLogMessages.ProccessStarted(_commandFullName, request.AccessToken));

                var httpContext = _httpContextAccessor.HttpContext;
                LogoutOthersRequestDto logoutOthersRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, logoutOthersRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(logoutOthersRequestDto.AccessToken),
                    
                        // Executors
                        ctx => _authBusinessLogicHelper.BlacklistSessionsAsync(logoutOthersRequestDto.AccessToken, BlacklistMode.AllExceptOne)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.LogoutOthersFailed,
                        methodName: nameof(LogoutOthersCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<bool>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.LogoutOthersSucceeded,
                    methodName: nameof(LogoutOthersCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: logoutOthersRequestDto.AccessToken,
                    isEventSuccess: true);

                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName, request.AccessToken));
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message }, 
                    statusCode: 500);
            }
        }
    }
}
