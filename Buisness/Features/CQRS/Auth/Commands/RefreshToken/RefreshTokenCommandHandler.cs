using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, BaseResponse<RefreshTokenResponseDto>>
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

        public async Task<BaseResponse<RefreshTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Refresh token işlemi başlatıldı. Access Token: {AccessToken}", request.AccessToken);

                RefreshTokenRequestDto mappedRequestData = new();
                RefreshTokenResponseDto refreshTokenResponseDto = new();

                var validationResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.MapToDtoAsync(request, mappedRequestData),
                    string.IsNullOrEmpty(mappedRequestData.AccessToken) || string.IsNullOrWhiteSpace(mappedRequestData.AccessToken)
                        ? new BuisnessLogicSuccessResult("AccessToken not required also not given", 200)
                        : await _authBusinessLogicHelper.IsAccessTokenValidAsync(mappedRequestData.AccessToken),
                    await _authBusinessLogicHelper.IsRefreshTokenValidAsync(mappedRequestData, refreshTokenResponseDto),
                    await _authBusinessLogicHelper.RefreshAccessTokenAsync(refreshTokenResponseDto)
                );

                if (validationResult != null)
                {
                    return BaseResponse<RefreshTokenResponseDto>.Failure(
                        message: validationResult.Message ?? "Refresh token işlemi sırasında hata oluştu",
                        statusCode: validationResult.StatusCode);
                }

                _logger.LogInformation("Refresh token işlemi başarılı. Yeni Access Token: {AccessToken}, Yeni Refresh Token: {RefreshToken}",
                    refreshTokenResponseDto.AccessToken, refreshTokenResponseDto.RefreshToken);
                return BaseResponse<RefreshTokenResponseDto>.Success(
                    data: refreshTokenResponseDto,
                    message: "Refresh token işlemi başarılı",
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu"); 
                return BaseResponse<RefreshTokenResponseDto>.Failure("Refresh token işlemi sırasında hata oluştu",
                    new List<string> { ex.Message }, 500);
            }
        }
    }
}
