using Buisness.Features.CQRS.Base;
using Buisness.Helpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, BaseResponse<RefreshTokenCommand>>
    {
        private readonly IAuthBuissnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<RefreshTokenCommand> _logger;

        public RefreshTokenCommandHandler(
            IAuthBuissnessLogicHelper authBusinessLogicHelper,
            ILogger<RefreshTokenCommand> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<RefreshTokenCommand>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Refresh token işlemi başlatıldı. Access Token: {AccessToken}", request.AccessToken);

                var validationResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.IsAccessTokenValidAsync(request)
                );

                if (validationResult!= null)
                {
                    return BaseResponse<RefreshTokenCommand>.Failure(
                        message: validationResult.Message ?? "Validation hatası",
                        statusCode: validationResult.StatusCode);
                }

                var refreshTokenResult = await _authBusinessLogicHelper.RefreshAccessToken(request);

                if (!refreshTokenResult.Success)
                {
                    return BaseResponse<RefreshTokenCommand>.Failure(
                        message: refreshTokenResult.Message ?? "Refresh token işlemi sırasında hata oluştu",
                        statusCode: refreshTokenResult.StatusCode);
                }

                _logger.LogInformation("Refresh token işlemi başarılı. Yeni AccessToken: {Access Token}", refreshTokenResult);

                return BaseResponse<RefreshTokenCommand>.Success(
                    data: refreshTokenResult.Data,
                    message: refreshTokenResult.Message ?? "Refresh token işlemi başarılı",
                    statusCode: refreshTokenResult.StatusCode);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu"); 
                return BaseResponse<RefreshTokenCommand>.Failure("Refresh token işlemi sırasında hata oluştu",
                    new List<string> { ex.Message }, 500);
            }
        }
    }
}
