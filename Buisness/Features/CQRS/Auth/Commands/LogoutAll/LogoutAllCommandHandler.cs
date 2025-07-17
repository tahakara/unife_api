using Buisness.Features.CQRS.Base;
using Buisness.Helpers.Auth;
using Core.Utilities.BuisnessLogic;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.LogoutAll
{
    public class LogoutAllCommandHandler : ICommandHandler<LogoutAllCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuissnessLogicHelper _authBuissnessLogicHelper;
        private readonly ILogger<LogoutAllCommand> _logger;

        public LogoutAllCommandHandler(
            IAuthBuissnessLogicHelper authbusinessLogicHelper,
            ILogger<LogoutAllCommand> logger)
        {
            _authBuissnessLogicHelper = authbusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("LogoutAll işlemi başlatıldı. UserUuid: {UserUuid}", request.UserUuid);
                var buisnessResult = BuisnessLogic.Run(
                    await _authBuissnessLogicHelper.IsAccessTokenValidAsync(request.AccessToken),
                    await _authBuissnessLogicHelper.BlacklistAllSessionTokensByUserAsync(request.UserUuid)
                );

                if (buisnessResult != null)
                {
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "LogoutAll işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

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
