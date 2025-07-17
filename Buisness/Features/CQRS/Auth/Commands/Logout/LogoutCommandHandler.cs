using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.Auth;
using Core.Utilities.BuisnessLogic;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuissnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<LogoutCommand> _logger;

        public LogoutCommandHandler(
            IAuthBuissnessLogicHelper authBuissnessLogicHelper,
            ILogger<LogoutCommand> logger)
        {
            _authBusinessLogicHelper = authBuissnessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Logout işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);

                var buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.IsAccessTokenValidAsync(request),
                    await _authBusinessLogicHelper.BlacklistSeesionTokensAsync(request.UserUuid, request.SessionUuid)
                );

                if (buisnessResult != null)
                {
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "Logout işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

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
