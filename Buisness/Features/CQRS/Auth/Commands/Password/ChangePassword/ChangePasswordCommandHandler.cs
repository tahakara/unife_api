using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Buisness.Helpers.HelperEnums;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword
{
    public class ChangePasswordCommandHandler : AuthCommandHandlerBase<ChangePasswordCommand>, ICommandHandler<ChangePasswordCommand, BaseResponse<bool>>
    {
        public ChangePasswordCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ChangePasswordCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "CahngePassword")
        {
        }

        public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(CQRSLogMessages.ProccessStarted(_commandFullName));

                var httpContext = _httpContextAccessor.HttpContext;

                ChangePasswordRequestDto changePasswordRequestDto = new();
                

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, changePasswordRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(changePasswordRequestDto.AccessToken),
                        ctx => _authBusinessLogicHelper.ValidatePasswordAsync(changePasswordRequestDto.AccessToken, changePasswordRequestDto.OldPassword),
                        
                        // Executors
                        ctx => _authBusinessLogicHelper.ChangePasswordAsync(changePasswordRequestDto.AccessToken, changePasswordRequestDto.OldPassword,     changePasswordRequestDto.NewPassword),

                        changePasswordRequestDto.LogoutOtherSessions
                            ? ctx => _authBusinessLogicHelper.BlacklistSessionsAsync(changePasswordRequestDto.AccessToken, BlacklistMode.AllExceptOne)
                            : ctx => Task.FromResult<IBuisnessLogicResult>(new BuisnessLogicSuccessResult())
                    });


                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext: httpContext,
                        accessToken: changePasswordRequestDto.AccessToken,
                        eventTypeGuidKey: SecurityEventTypeGuid.PasswordChange,
                        methodName: nameof(ChangePasswordCommandHandler),
                        description: _commandFullName,
                        isEventSuccess:  false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown
                    );
                    return BaseResponse<bool>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }

                _logger.LogDebug(CQRSLogMessages.ProccessCompleted(_commandFullName));

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext: httpContext,
                    accessToken: changePasswordRequestDto.AccessToken,
                    eventTypeGuidKey: SecurityEventTypeGuid.PasswordChange,
                    methodName: nameof(ChangePasswordCommandHandler),
                    description: _commandFullName,
                    isEventSuccess: true
                );
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName));

            }
            catch (Exception ex)
            {
                _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors : new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}

