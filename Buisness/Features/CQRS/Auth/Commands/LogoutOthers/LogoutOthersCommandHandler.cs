using Buisness.Features.CQRS.Base;
using Buisness.Helpers.Auth;
using Core.Utilities.BuisnessLogic;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.LogoutOthers
{
    public class LogoutOthersCommandHandler : ICommandHandler<LogoutOthersCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuissnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<LogoutOthersCommand> _logger;

        public LogoutOthersCommandHandler(
            IAuthBuissnessLogicHelper authBusinessLogicHelper,
            ILogger<LogoutOthersCommand> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutOthersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("LogoutOthers işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);
                var buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.IsAccessTokenValidAsync(request),
                    await _authBusinessLogicHelper.BlacklistSessionsExcludedByOneAsync(request.UserUuid, request.SessionUuid, request.SessionUuid)
                );

                if (buisnessResult == null)
                {
                    return BaseResponse<bool>.Success(
                        data: true,
                        message: "LogoutOthers işlemi başarılı");
                }
                return BaseResponse<bool>.Failure(
                    message: buisnessResult.Message ?? "LogoutOthers işlemi sırasında hata oluştu",
                    statusCode: buisnessResult.StatusCode);

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
