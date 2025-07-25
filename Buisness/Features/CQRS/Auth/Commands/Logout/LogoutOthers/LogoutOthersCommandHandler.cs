using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
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
                LogoutOthersRequestDto mappedRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, mappedRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(mappedRequestDto.AccessToken),
                    
                        // Executors
                        ctx => _authBusinessLogicHelper.BlacklistSessionsExcludedByOneAsync(mappedRequestDto.AccessToken)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext: httpContext,
                        accessToken: mappedRequestDto.AccessToken,
                        eventTypeGuidKey: SecurityEventTypeGuid.LogoutOthers,
                        methodName: nameof(LogoutOthersCommandHandler),
                        description: _commandFullName,
                        isEventSuccess:  false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown
                    );
                    return BaseResponse<bool>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext: httpContext,
                    accessToken: mappedRequestDto.AccessToken,
                    eventTypeGuidKey: SecurityEventTypeGuid.LogoutOthers,
                    methodName: nameof(LogoutOthersCommandHandler),
                    description: _commandFullName,
                    isEventSuccess: true
                );
                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName, request.AccessToken));
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
                    errors: new List<string> { ex.Message }, 
                    statusCode: 500);
            }
        }
    }
}
