using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll
{
    public class LogoutAllCommandHandler : ICommandHandler<LogoutAllCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuisnessLogicHelper _authBuissnessLogicHelper;
        private readonly ILogger<LogoutAllCommand> _logger;

        public LogoutAllCommandHandler(
            IAuthBuisnessLogicHelper authbusinessLogicHelper,
            ILogger<LogoutAllCommand> logger)
        {
            _authBuissnessLogicHelper = authbusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Logout işlemi başarılı. AccessToken: {AccessToken}", request.AccessToken);

                LogoutAllRequestDto mappedRequestDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBuissnessLogicHelper.ValidateCommandAsync(request),
                    await _authBuissnessLogicHelper.MapToDtoAsync(request, mappedRequestDto),
                    await _authBuissnessLogicHelper.IsAccessTokenValidAsync(mappedRequestDto.AccessToken),
                    await _authBuissnessLogicHelper.BlacklistAllSessionTokensByUserAsync(mappedRequestDto.AccessToken)
                );

                if (buisnessResult != null)
                {
                    _logger.LogDebug("LogoutAll işlemi sırasında hata oluştu: {Message}", buisnessResult.Message);
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "LogoutAll işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

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
