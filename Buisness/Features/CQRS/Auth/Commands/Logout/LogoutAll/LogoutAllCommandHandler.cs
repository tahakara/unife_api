using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
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
            : base(authbusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Logout işlemi başarılı. AccessToken: {AccessToken}", request.AccessToken);

                HttpContext? httpContext = _httpContextAccessor.HttpContext;

                LogoutAllRequestDto logoutAllRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.ValidateAsync(request),
                    () => _authBusinessLogicHelper.MapToDtoAsync(request, logoutAllRequestDto),
                    () => _authBusinessLogicHelper.IsAccessTokenValidAsync(logoutAllRequestDto.AccessToken)
                );

                if (buisnessResult != null)
                    return BaseResponse<bool>.Failure(
                            message: buisnessResult.Message ?? "LogoutAll işlemi sırasında hata oluştu",
                            statusCode: buisnessResult.StatusCode);


                IBuisnessLogicResult blackListResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.BlacklistAllSessionTokensByUserAsync(logoutAllRequestDto.AccessToken)
                );
                if (blackListResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext,
                        logoutAllRequestDto.AccessToken,
                        SecurityEventTypeGuid.Logout,
                        nameof(LogoutCommandHandler),
                        "Logout All",
                        false,
                        blackListResult.Message ?? "Logout işlemi sırasında hata oluştu"
                    );
                    return BaseResponse<bool>.Failure(
                        message: blackListResult.Message ?? "LogoutAll işlemi sırasında hata oluştu",
                        statusCode: blackListResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext,
                    logoutAllRequestDto.AccessToken,
                    SecurityEventTypeGuid.Logout,
                    nameof(LogoutAllCommandHandler),
                    "Logout All",
                    true
                );
                _logger.LogDebug("LogoutAll işlemi başarılı. AccessToken: {AccessToken}", request.AccessToken);
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "LogoutAll işlemi başarılı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogoutAll işlemi sırasında hata oluştu");
                return BaseResponse<bool>.Failure("LogoutAll işlemi sırasında hata oluştu",
                    new List<string> { ex.Message }, 500);
            }
        }
    }
}
