using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : AuthCommandHandlerBase<RefreshTokenCommand>, ICommandHandler<RefreshTokenCommand, BaseResponse<RefreshTokenResponseDto>>
    {
        public RefreshTokenCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<RefreshTokenCommand> logger) 
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<RefreshTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Refresh token işlemi başlatıldı. Access Token: {AccessToken}", request.AccessToken);

                var httpContext = _httpContextAccessor.HttpContext;

                RefreshTokenRequestDto refreshTokenRequestDto = new();
                RefreshTokenResponseDto refreshTokenResponseDto = new();

                IBuisnessLogicResult validationResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.ValidateAsync(request),
                    () => _authBusinessLogicHelper.MapToDtoAsync(request, refreshTokenRequestDto),
                    //() => string.IsNullOrEmpty(refreshTokenRequestDto.AccessToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.AccessToken)
                    //    ? new BuisnessLogicSuccessResult("AccessToken not required also not given", 200)
                    //    : await _authBusinessLogicHelper.IsAccessTokenValidAsync(refreshTokenRequestDto.AccessToken),
                    () => _authBusinessLogicHelper.IsRefreshTokenValidAsync(refreshTokenRequestDto, refreshTokenResponseDto)
                );

                if (validationResult != null)
                    return BaseResponse<RefreshTokenResponseDto>.Failure(
                        message: validationResult.Message ?? "Refresh token işlemi sırasında hata oluştu",
                        statusCode: validationResult.StatusCode);


                IBuisnessLogicResult refreshAccessTokenResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.RefreshAccessTokenAsync(refreshTokenResponseDto)
                );
                if (refreshAccessTokenResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext,
                        refreshTokenResponseDto.AccessToken,
                        SecurityEventTypeGuid.SessionRefreshed,
                        nameof(RefreshTokenCommandHandler),
                        "Refresh Token",
                        false,
                        refreshAccessTokenResult.Message ?? "Refresh token işlemi sırasında hata oluştu"
                    );
                    return BaseResponse<RefreshTokenResponseDto>.Failure(
                            message: refreshAccessTokenResult.Message ?? "Refresh token işlemi sırasında hata oluştu",
                            statusCode: refreshAccessTokenResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext,
                    refreshTokenResponseDto.AccessToken,
                    SecurityEventTypeGuid.SessionRefreshed,
                    nameof(RefreshTokenCommandHandler),
                    "Refresh Token",
                    true
                );
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
