using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.LogoutOthers
{
    public class LogoutOthersCommandHandler : ICommandHandler<LogoutOthersCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<LogoutOthersCommand> _logger;

        public LogoutOthersCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            ILogger<LogoutOthersCommand> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutOthersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("LogoutOthers işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);

                LogoutOthersRequestDto mappedRequestDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateCommandAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, mappedRequestDto),
                    await _authBusinessLogicHelper.IsAccessTokenValidAsync(mappedRequestDto.AccessToken),
                    await _authBusinessLogicHelper.BlacklistSessionsExcludedByOneAsync(mappedRequestDto.AccessToken)
                );

                if (buisnessResult != null)
                {
                    _logger.LogDebug("LogoutOthers işlemi sırasında hata oluştu: {Message}", buisnessResult.Message);
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "LogoutOthers işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

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
