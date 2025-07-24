using Buisness.DTOs.AuthDtos.RefreshDtos;
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

namespace Buisness.Features.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : AuthCommandHandlerBase<RefreshTokenCommand>, ICommandHandler<RefreshTokenCommand, BaseResponse<RefreshTokenResponseDto>>
    {
        public RefreshTokenCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<RefreshTokenCommand> logger) 
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "RefreshToken")
        {
        }

        public async Task<BaseResponse<RefreshTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(CQRSLogMessages.ProccessStarted(_commandFullName, request.AccessToken));

                var httpContext = _httpContextAccessor.HttpContext;

                RefreshTokenRequestDto refreshTokenRequestDto = new();
                RefreshTokenResponseDto refreshTokenResponseDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, refreshTokenRequestDto),
                        //ctx => string.IsNullOrEmpty(refreshTokenRequestDto.AccessToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.AccessToken)
                        //    ? new BuisnessLogicSuccessResult("AccessToken not required also not given", 200)
                        //    : await _authBusinessLogicHelper.IsAccessTokenValidAsync(refreshTokenRequestDto.AccessToken),
                        ctx => _authBusinessLogicHelper.IsRefreshTokenValidAsync(refreshTokenRequestDto, refreshTokenResponseDto),

                        // Executors
                        ctx => _authBusinessLogicHelper.RefreshAccessTokenAsync(refreshTokenResponseDto)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext: httpContext,
                        accessToken: refreshTokenResponseDto.AccessToken,
                        eventTypeGuidKey: SecurityEventTypeGuid.SessionRefreshed,
                        methodName: nameof(RefreshTokenCommandHandler),
                        description: _commandFullName,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSResponseMessages.Fail(_commandName, buisnessResult.Message)
                    );
                    return BaseResponse<RefreshTokenResponseDto>.Failure(
                            message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                            statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext: httpContext,
                    accessToken: refreshTokenResponseDto.AccessToken,
                    eventTypeGuidKey: SecurityEventTypeGuid.SessionRefreshed,
                    methodName: nameof(RefreshTokenCommandHandler),
                    description: _commandFullName,
                    isEventSuccess: true
                );

                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName, refreshTokenResponseDto.RefreshToken));
                return BaseResponse<RefreshTokenResponseDto>.Success(
                    data: refreshTokenResponseDto,
                    message: CQRSResponseMessages.Success(_commandName, refreshTokenResponseDto.AccessToken),
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request.AccessToken));
                return BaseResponse<RefreshTokenResponseDto>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message }, 
                    statusCode: 500);
            }
        }
    }
}
