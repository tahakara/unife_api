using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, BaseResponse<bool>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<LogoutCommand> _logger;

        public LogoutCommandHandler(
            IAuthBuisnessLogicHelper authBuissnessLogicHelper,
            ILogger<LogoutCommand> logger)
        {
            _authBusinessLogicHelper = authBuissnessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Logout işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);

                LogoutRequestDto mappedRequestDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateCommandAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, mappedRequestDto),
                    await _authBusinessLogicHelper.IsAccessTokenValidAsync(mappedRequestDto.AccessToken)
                );

                if (buisnessResult != null)
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "Logout işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);


                IBuisnessLogicResult blackListResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.BlackListSessionTokensForASingleSessionAsync(mappedRequestDto.AccessToken)
                );
                if (blackListResult != null)
                    return BaseResponse<bool>.Failure(
                        message: blackListResult.Message ?? "Logout işlemi sırasında hata oluştu",
                        statusCode: blackListResult.StatusCode);


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
