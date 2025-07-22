using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
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
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(LogoutOthersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("LogoutOthers işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);

                var httpContext = _httpContextAccessor.HttpContext;
                LogoutOthersRequestDto mappedRequestDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, mappedRequestDto),
                    await _authBusinessLogicHelper.IsAccessTokenValidAsync(mappedRequestDto.AccessToken)
                );

                if (buisnessResult != null)
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "LogoutOthers işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);


                IBuisnessLogicResult blackListResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.BlacklistSessionsExcludedByOneAsync(mappedRequestDto.AccessToken)
                );
                if (blackListResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext,
                        mappedRequestDto.AccessToken,
                        SecurityEventTypeGuid.LogoutOthers,
                        nameof(LogoutOthersCommandHandler),
                        "Logout Others",
                        false,
                        blackListResult.Message ?? "LogoutOthers işlemi sırasında hata oluştu"
                    );
                    return BaseResponse<bool>.Failure(
                        message: blackListResult.Message ?? "LogoutOthers işlemi sırasında hata oluştu",
                        statusCode: blackListResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext,
                    mappedRequestDto.AccessToken,
                    SecurityEventTypeGuid.LogoutOthers,
                    nameof(LogoutOthersCommandHandler),
                    "Logout Others",
                    true
                );
                _logger.LogDebug("LogoutOthers işlemi başarılı. Access Token: {AccessToken}", request.AccessToken);
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "LogoutOthers işlemi başarılı",
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogoutOthers işlemi sırasında hata oluştu");
                return BaseResponse<bool>.Failure("LogoutOthers işlemi sırasında hata oluştu",
                    new List<string> { ex.Message }, 500);
            }
        }
    }
}
