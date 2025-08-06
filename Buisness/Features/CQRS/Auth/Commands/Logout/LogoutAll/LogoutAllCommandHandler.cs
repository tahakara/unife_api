using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
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

namespace Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll
{
    public class LogoutAllCommandHandler : AuthCommandHandlerBase<LogoutAllCommand>, ICommandHandler<LogoutAllCommand, BaseResponse<bool>>
    {
        public LogoutAllCommandHandler(
            IAuthBuisnessLogicHelper authbusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogoutAllCommand> logger) 
            : base(authbusinessLogicHelper, httpContextAccessor, logger, "LogoutAll")
        {
        }

        public async Task<BaseResponse<bool>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName, request.AccessToken));

                HttpContext? httpContext = _httpContextAccessor.HttpContext;

                LogoutAllRequestDto logoutAllRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, logoutAllRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(logoutAllRequestDto.AccessToken),

                        // Executors
                        ctx => _authBusinessLogicHelper.BlacklistSessionsAsync(logoutAllRequestDto.AccessToken, BlacklistMode.All) 
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.LogoutAllFailed,
                        methodName: nameof(LogoutAllCommandHandler),
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
                        eventTypeGuidKey: SecurityEventTypeGuid.LogoutAllSucceeded,
                        methodName: nameof(LogoutAllCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: logoutAllRequestDto.AccessToken,
                        isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
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
