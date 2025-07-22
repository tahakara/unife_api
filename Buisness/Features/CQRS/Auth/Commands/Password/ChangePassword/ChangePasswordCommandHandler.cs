using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
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
                    () => _authBusinessLogicHelper.ValidateAsync(request),
                    () => _authBusinessLogicHelper.MapToDtoAsync(request, changePasswordRequestDto),
                    () => _authBusinessLogicHelper.IsAccessTokenValidAsync(changePasswordRequestDto.AccessToken),
                    () => _authBusinessLogicHelper.CheckPasswordIsCorrect(changePasswordRequestDto.AccessToken, changePasswordRequestDto.OldPassword));

                if (buisnessResult != null)
                {
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "ChangePassword işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

                IBuisnessLogicResult changePasswordResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.ChangePasswordAsync(changePasswordRequestDto.AccessToken, changePasswordRequestDto.OldPassword, changePasswordRequestDto.NewPassword),
                    () => _authBusinessLogicHelper.BlacklistOtherSessionsAfterPasswordChangeAsync(changePasswordRequestDto.AccessToken, changePasswordRequestDto.LogoutOtherSessions));

                if (changePasswordResult != null)
                {
                    return BaseResponse<bool>.Failure(
                        message: changePasswordResult.Message ?? "ChangePassword işlemi sırasında hata oluştu",
                        statusCode: changePasswordResult.StatusCode);
                }

                _logger.LogInformation("ChangePassword işlemi başarılı");

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

