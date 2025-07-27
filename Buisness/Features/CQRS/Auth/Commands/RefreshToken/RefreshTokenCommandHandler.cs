using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Enums;
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
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.SessionRefreshFalied,
                        methodName: nameof(RefreshTokenCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogDebug(CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<RefreshTokenResponseDto>.Failure(
                            message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                            statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.SessionRefreshSucceeded,
                    methodName: nameof(RefreshTokenCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: refreshTokenRequestDto.AccessToken,
                    isEventSuccess: true);

                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<RefreshTokenResponseDto>.Success(
                    data: refreshTokenResponseDto,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request));
                return BaseResponse<RefreshTokenResponseDto>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message }, 
                    statusCode: 500);
            }
        }
    }
}
