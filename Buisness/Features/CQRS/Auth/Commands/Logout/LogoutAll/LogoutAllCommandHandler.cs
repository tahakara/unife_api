using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll
{
    public class LogoutAllCommandHandler : ICommandHandler<LogoutAllCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuisnessLogicHelper _authBuissnessLogicHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LogoutAllCommand> _logger;

        public LogoutAllCommandHandler(
            IAuthBuisnessLogicHelper authbusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LogoutAllCommand> logger)
        {
            _authBuissnessLogicHelper = authbusinessLogicHelper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Logout işlemi başarılı. AccessToken: {AccessToken}", request.AccessToken);

                HttpContext? httpContext = _httpContextAccessor.HttpContext;

                LogoutAllRequestDto logoutAllRequestDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBuissnessLogicHelper.ValidateAsync(request),
                    await _authBuissnessLogicHelper.MapToDtoAsync(request, logoutAllRequestDto),
                    await _authBuissnessLogicHelper.IsAccessTokenValidAsync(logoutAllRequestDto.AccessToken)
                );

                if (buisnessResult != null)
                {
                    //await _authBuissnessLogicHelper.AddLogoutAllSecurityEventRecordAsync(httpContext, logoutAllRequestDto, false, buisnessResult.Message);
                    return BaseResponse<bool>.Failure(
                            message: buisnessResult.Message ?? "LogoutAll işlemi sırasında hata oluştu",
                            statusCode: buisnessResult.StatusCode);
                }


                IBuisnessLogicResult blackListResult = BuisnessLogic.Run(

                    //await _authBuissnessLogicHelper.AddLogoutAllSecurityEventRecordAsync(httpContext, logoutAllRequestDto, false),
                    await _authBuissnessLogicHelper.BlacklistAllSessionTokensByUserAsync(logoutAllRequestDto.AccessToken)
                );
                if (blackListResult != null)
                {
                    //await _authBuissnessLogicHelper.AddLogoutAllSecurityEventRecordAsync(httpContext, logoutAllRequestDto, false, blackListResult.Message);
                    return BaseResponse<bool>.Failure(
                        message: blackListResult.Message ?? "LogoutAll işlemi sırasında hata oluştu",
                        statusCode: blackListResult.StatusCode);
                }


                _logger.LogDebug("LogoutAll işlemi başarılı. AccessToken: {AccessToken}", request.AccessToken);
                //await _authBuissnessLogicHelper.AddLogoutAllSecurityEventRecordAsync(httpContext, logoutAllRequestDto, true);
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
