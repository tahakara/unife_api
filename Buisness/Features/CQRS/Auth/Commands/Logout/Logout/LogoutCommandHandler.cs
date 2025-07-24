using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Enums;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.Logout
{
    public class LogoutCommandHandler : AuthCommandHandlerBase<LogoutCommand>, ICommandHandler<LogoutCommand, BaseResponse<bool>>
    {
        public LogoutCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogoutCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Logout işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);

                var httpContext = _httpContextAccessor.HttpContext;

                LogoutRequestDto logoutRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, logoutRequestDto),
                        ctx => _authBusinessLogicHelper.IsAccessTokenValidAsync(logoutRequestDto.AccessToken),

                        // Executors
                        ctx => _authBusinessLogicHelper.BlackListSessionTokensForASingleSessionAsync(logoutRequestDto.AccessToken)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext,
                        logoutRequestDto.AccessToken,
                        SecurityEventTypeGuid.Logout,
                        nameof(LogoutCommandHandler),
                        "Logout",
                        false,
                        buisnessResult.Message ?? "Logout işlemi sırasında hata oluştu"
                    );
                    return BaseResponse<bool>.Failure(
                            message: buisnessResult.Message ?? "Logout işlemi sırasında hata oluştu",
                            statusCode: buisnessResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext,
                    logoutRequestDto.AccessToken,
                    SecurityEventTypeGuid.Logout,
                    nameof(LogoutCommandHandler),
                    "Logout",
                    true
                );
                _logger.LogDebug("Logout işlemi başarılı. AccessToken: {AccessToken}", request.AccessToken);
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "Logout işlem başarılı");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout işlemi sırasında hata oluştu");
                return BaseResponse<bool>.Failure("Logout işlemi sırasında hata oluştu",
                    new List<string> { ex.Message }, 500);
            }

        }
    }
}
