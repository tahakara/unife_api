using AutoMapper;
using Buisness.Abstract.DtoBase.Base;
using Buisness.Abstract.ServicesBase;
using Buisness.Concrete.Dto;
using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Services.UtilityServices;
using Buisness.Services.UtilityServices.Abtract;
using Core.Utilities.BuisnessLogic.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.DataResults;
using DataAccess.Abstract;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Buisness.Helpers.BuisnessLogicHelpers.Auth
{
    public interface IAuthBuissnessLogicHelper : IBuisnessLogicHelper
    {
        //Task<IBuisnessLogicResult> YouNeedAnAccessToken();
        Task<IBuisnessLogicResult> MapToDtoAsync<TSource, TDestination>(TSource source, TDestination target)
            where TSource : class, new()
            where TDestination : class, new();
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken);
        Task<IBuisnessLogicResult> YouCanNotComeToThisEndPointWithAnyAccessTokenAsync(TokenCommandBase request);
        Task<IBuisnessLogicResult> BlackListSessionTokensForASingleSessionAsync(string accessToken);
        Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string accessToken);
        Task<IBuisnessLogicResult> BlacklistSessionsExcludedByOneAsync(string accessToken);
        Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto);
        Task<IBuisnessLogicResult> RefreshAccessTokenAsync(RefreshTokenResponseDto refreshTokenResponseDto);
        Task<IBuisnessLogicResult> CheckUserTypeAsync(int? userType);
        Task<IBuisnessLogicResult> CheckLoginCredentialsAsync(SignInCommand request);

    }
    public class AuthBuissnessLogicHelper : IAuthBuissnessLogicHelper
    {
        private readonly ISessionJwtService _sessionJwtService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthBuissnessLogicHelper> _logger;

        public AuthBuissnessLogicHelper(
            ISessionJwtService sessionJwtService,
            IMapper mapper,
            ILogger<AuthBuissnessLogicHelper> logger)
        {
            _sessionJwtService = sessionJwtService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IBuisnessLogicResult> MapToDtoAsync<TSource, TDestination>(TSource source, TDestination target)
            where TSource : class, new()
            where TDestination : class, new()
        {
            try
            {
                _logger.LogDebug("Mapping {SourceType} to {TargetType} started",
                    typeof(TSource).Name, typeof(TDestination).Name);

                if (source == null || target == null)
                {
                    return new BuisnessLogicErrorResult("Mapping failed: null input", 400);
                }

                _mapper.Map(source, target); // AutoMapper: source -> destination

                _logger.LogDebug("Mapping {SourceType} to {TargetType} completed successfully",
                    typeof(TSource).Name, typeof(TDestination).Name);
                return new BuisnessLogicSuccessResult("Mapping successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mapping failed due to exception");
                return new BuisnessLogicErrorResult("Mapping failed", 500);
            }
        }

        public async Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken)
        {
            try
            {
                _logger.LogDebug("Validating access token {AccessToken}", accessToken);

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrWhiteSpace(accessToken))
                {
                    return new BuisnessLogicErrorResult("Access token is required", 400);
                }

                bool isValid = await _sessionJwtService.ValidateTokenAsync(accessToken);
                if (!isValid)
                {
                    return new BuisnessLogicErrorResult("AccessToken invalid or expired");
                }

                _logger.LogDebug("Access token validation successful for {AccessToken}", accessToken);
                return new BuisnessLogicSuccessResult("Access token validation successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Access token validation failed");
                return new BuisnessLogicErrorResult("Access token validation failed", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlackListSessionTokensForASingleSessionAsync(string accessToken)
        {
            try
            {
                _logger.LogDebug("Blacklisting access token {AccessToken}", accessToken);


                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var sessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(accessToken);

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are not found on AccessToken", 400);
                }

                var revokeResult = await _sessionJwtService.RevokeTokensAsync(userUuid, sessionUuid);
                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke tokens", 500);
                }

                _logger.LogDebug("Access token blacklisted successfully for user {UserUuid} and session {SessionUuid}", userUuid, sessionUuid);
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error blacklisting access token {AccessToken}", accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistSessionsExcludedByOneAsync(string accessToken)
        {
            try
            {
                _logger.LogDebug("Blacklisting access token {AccessToken} excluding one session", accessToken);

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var currentSessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(accessToken);

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(currentSessionUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are not found on AccessToken", 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensExcluededByOne(userUuid, currentSessionUuid);

                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke tokens", 500);
                }

                _logger.LogDebug("Access token blacklisted successfully for user {UserUuid} excluding session {cURRENTSessionUuid}", userUuid, currentSessionUuid);
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error blacklisting access token {AccessToken}", accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string accessToken)
        {
            try
            {
                _logger.LogDebug("Blacklisting all session tokens for user with access token {AccessToken}", accessToken);

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                if (string.IsNullOrEmpty(userUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID is not found on AccessToken", 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensAsync(userUuid);

                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke all session tokens", 500);
                }

                _logger.LogDebug("All session tokens blacklisted successfully for user {UserUuid}", userUuid);
                return new BuisnessLogicSuccessResult("All session tokens blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error blacklisting all session tokens for user with access token {AccessToken}", accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting all session tokens by user", 500);
            }
        }

        public async Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto)
        {
            try
            {
                _logger.LogDebug("IsRefreshTokenValid işlemi başlatıldı. Refresh Token: {RefreshToken}", refreshTokenRequestDto.RefreshToken);

                if (string.IsNullOrEmpty(refreshTokenRequestDto.RefreshToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.RefreshToken))
                {
                    return new BuisnessLogicErrorResult("Refresh token is required", 400);
                }

                if (string.IsNullOrEmpty(refreshTokenRequestDto.AccessToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.AccessToken))
                {
                    var postfixSearchKey = await _sessionJwtService.GetRefreshTokenKeyByRefreshTokenPostfixAsync(refreshTokenRequestDto.RefreshToken);

                    if (string.IsNullOrEmpty(postfixSearchKey))
                    {
                        return new BuisnessLogicErrorResult("Refresh token not found", 400);
                    }

                    JsonElement? postfixSearchResult = await _sessionJwtService.GetRefreshTokenValue(postfixSearchKey);
                    if (postfixSearchKey == null)
                    {
                        return new BuisnessLogicErrorResult("Refresh token not found", 400);
                    }

                    if (!postfixSearchResult.HasValue || !postfixSearchResult.Value.TryGetProperty("UserUuid", out var userUuid) || !postfixSearchResult.Value.TryGetProperty("SessionUuid", out var sessionUuid))
                    {
                        return new BuisnessLogicErrorResult("Refresh token is not valid or expired", 400);
                    }

                    refreshTokenResponseDto.UserUuid = userUuid.GetString() ?? string.Empty;
                    refreshTokenResponseDto.SessionUuid = sessionUuid.GetString() ?? string.Empty;
                    refreshTokenResponseDto.RefreshToken = refreshTokenRequestDto.RefreshToken;

                    return new BuisnessLogicSuccessResult("Refresh token is valid", 200);

                }

                refreshTokenResponseDto.UserUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(refreshTokenRequestDto.AccessToken);
                refreshTokenResponseDto.SessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(refreshTokenRequestDto.AccessToken);
                refreshTokenResponseDto.RefreshToken = refreshTokenRequestDto.RefreshToken;
                if (string.IsNullOrEmpty(refreshTokenResponseDto.UserUuid) || string.IsNullOrEmpty(refreshTokenResponseDto.SessionUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are not found on AccessToken", 400);
                }

                return new BuisnessLogicSuccessResult("Refresh token is valid", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IsRefreshTokenValid işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("IsRefreshTokenValid işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> RefreshAccessTokenAsync(RefreshTokenResponseDto refreshTokenResponseDto)
        {
            try
            {
                _logger.LogDebug("Refresh token işlemi başlatıldı. Refresh Token: {RefreshToken}", refreshTokenResponseDto.RefreshToken);

                var refreshResult = await _sessionJwtService.RefreshAccessTokenAsync(refreshTokenResponseDto.UserUuid, refreshTokenResponseDto.SessionUuid, refreshTokenResponseDto.RefreshToken);
                if (refreshResult == null)
                {
                    return new BuisnessLogicErrorResult("Refresh token işlemi başarısız oldu", 500);
                }

                refreshTokenResponseDto.AccessToken = refreshResult.AccessToken;
                refreshTokenResponseDto.RefreshToken = refreshResult.RefreshToken;

                _logger.LogDebug("Refresh token işlemi başarılı. Yeni Access Token: {AccessToken}, Yeni Refresh Token: {RefreshToken }", refreshTokenResponseDto.AccessToken, refreshTokenResponseDto.RefreshToken);
                return new BuisnessLogicSuccessResult("Refresh token işlemi başarılı", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("Refresh token işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> YouCanNotComeToThisEndPointWithAnyAccessTokenAsync(TokenCommandBase request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _logger.LogDebug("YouCanNotComeToThisEndPointWithAnyAccessTokenAsync işlemi başlatıldı. AccessToken: {AccessToken}", request.AccessToken);
                    if (string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrWhiteSpace(request.AccessToken))
                    {
                        _logger.LogDebug("Access token is required for this endpoint. Access Token: {AccessToken}", request.AccessToken);
                        return new BuisnessLogicErrorResult();
                    }

                    return new BuisnessLogicErrorResult("Bu endpointe access token ile gelemezsin", 409);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "YouCanNotComeToThisEndPointWithAnyAccessTokenAsync işlemi sırasında hata oluştu");
                    return new BuisnessLogicErrorResult("YouCanNotComeToThisEndPointWithAnyAccessTokenAsync işlemi sırasında hata oluştu", 500);
                }
            });
        }

        public async Task<IBuisnessLogicResult> CheckUserTypeAsync(int? userType)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _logger.LogDebug("CheckUserType işlemi başlatıldı. UserType: {UserType}", userType);
                    switch (userType)
                    {
                        case (int)UserType.Admin:
                            _logger.LogDebug("Kullanıcı tipi: Admin");
                            return (IBuisnessLogicResult)new BuisnessLogicSuccessResult("Kullanıcı tipi: Admin", 200);
                        case (int)UserType.Staff:
                            _logger.LogDebug("Kullanıcı tipi: Staff");
                            return (IBuisnessLogicResult)new BuisnessLogicSuccessResult("Kullanıcı tipi: Staff", 200);
                        case (int)UserType.User:
                            _logger.LogDebug("Kullanıcı tipi: User");
                            return (IBuisnessLogicResult)new BuisnessLogicSuccessResult("Kullanıcı tipi: User", 200);
                        default:
                            return new BuisnessLogicErrorResult("Geçersiz kullanıcı tipi", 400);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CheckUserType işlemi sırasında hata oluştu");
                    return new BuisnessLogicErrorResult("CheckUserType işlemi sırasında hata oluştu", 500);
                }
            });
        }

        public async Task<IBuisnessLogicResult> CheckLoginCredentialsAsync(SignInCommand request)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _logger.LogDebug("CheckLoginCredentials işlemi başlatıldı. Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                        request.Email, request.PhoneCountryCode, request.PhoneNumber);

                    if (string.IsNullOrEmpty(request.Email))
                    {
                        return new BuisnessLogicErrorResult("Email veya PhoneCountryCode ve PhoneNumber alanlarından en az biri gereklidir", 400);
                    }

                    return new BuisnessLogicErrorResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CheckLoginCredentials işlemi sırasında hata oluştu");
                    return new BuisnessLogicErrorResult("CheckLoginCredentials işlemi sırasında hata oluştu", 500);
                }
            });
        }

        public enum UserType
        {
            Admin = 1,
            Staff = 2,
            User = 3,
            _ = 4
        }

    }
}
