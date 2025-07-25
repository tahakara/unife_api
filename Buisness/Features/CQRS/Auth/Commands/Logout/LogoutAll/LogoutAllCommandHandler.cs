using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
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
                        ctx => _authBusinessLogicHelper.BlacklistAllSessionTokensByUserAsync(logoutAllRequestDto.AccessToken) 
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext: httpContext,
                        accessToken: logoutAllRequestDto.AccessToken,
                        eventTypeGuidKey: SecurityEventTypeGuid.Logout,
                        methodName: nameof(LogoutCommandHandler),
                        description: _commandFullName,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown
                    );
                    return BaseResponse<bool>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext: httpContext,
                    accessToken: logoutAllRequestDto.AccessToken,
                    eventTypeGuidKey: SecurityEventTypeGuid.Logout,
                    methodName: nameof(LogoutAllCommandHandler),
                    description: "Logout All",
                    isEventSuccess: true
                );

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName));
            }
            catch (Exception ex)
            {
                _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request.AccessToken));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}
