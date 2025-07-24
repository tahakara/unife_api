using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
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
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("ChangePassword işlemi başlatıldı.");

                var httpContext = _httpContextAccessor.HttpContext;

                ChangePasswordRequestDto changePasswordRequestDto = new();
                

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, changePasswordRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(changePasswordRequestDto.AccessToken),
                        ctx => _authBusinessLogicHelper.CheckPasswordIsCorrect(changePasswordRequestDto.AccessToken, changePasswordRequestDto.OldPassword),
                        
                        // Executors
                        ctx => _authBusinessLogicHelper.ChangePasswordAsync(changePasswordRequestDto.AccessToken, changePasswordRequestDto.OldPassword,     changePasswordRequestDto.NewPassword),
                        ctx => _authBusinessLogicHelper.BlacklistOtherSessionsAfterPasswordChangeAsync(changePasswordRequestDto.AccessToken, changePasswordRequestDto.LogoutOtherSessions) 
                    });


                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext,
                        changePasswordRequestDto.AccessToken,
                        SecurityEventTypeGuid.PasswordChange,
                        nameof(ChangePasswordCommandHandler),
                        "Change Password",
                        false,
                        buisnessResult.Message ?? "ChangePassword işlemi sırasında hata oluştu"
                    );
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "ChangePassword işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

                _logger.LogInformation("ChangePassword işlemi başarılı");

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext,
                    changePasswordRequestDto.AccessToken,
                    SecurityEventTypeGuid.PasswordChange,
                    nameof(ChangePasswordCommandHandler),
                    "Change Password",
                    true
                );
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "ChangePassword işlemi başarılı");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangePassword işlemi sırasında hata oluştu");
                return BaseResponse<bool>.Failure(
                    message: "ChangePassword işlemi sırasında hata oluştu",
                    errors : new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}

