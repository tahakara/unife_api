using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
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
using System.Threading.Tasks;

namespace Buisness.Helpers.Auth
{
    public interface IAuthBuissnessLogicHelper : IBuisnessLogicHelper
    {
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken);
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(LogoutCommand request);
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(RefreshTokenCommand request);
        Task<IBuisnessLogicResult> BlacklistSeesionTokensAsync(string? userUuid, string? sessionUuid);
        Task<IBuisnessLogicResult> BlacklistSessionsExcludedByOneAsync(string? userUuid, string? sessionUuid, string? excludedSessionUuid);
        Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string? userUuid);

        Task<IBuisnessLogicDataResult<RefreshTokenCommand>> RefreshAccessToken(RefreshTokenCommand request);

    }
    public class AuthBuissnessLogicHelper : IAuthBuissnessLogicHelper
    {
        private readonly ISessionJwtService _sessionJwtService;
        private readonly ILogger<AuthBuissnessLogicHelper> _logger;

        public AuthBuissnessLogicHelper(
            ISessionJwtService sessionJwtService,
            ILogger<AuthBuissnessLogicHelper> logger)
        {
            _sessionJwtService = sessionJwtService;
            _logger = logger;
        }

        public async Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken)
        {
            try
            {
                _logger.LogInformation("Validating access token {AccessToken}", accessToken);

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrWhiteSpace(accessToken))
                {
                    return new BuisnessLogicErrorResult("Access token is required", 400);
                }

                bool isValid = await _sessionJwtService.ValidateTokenAsync(accessToken);
                if (!isValid)
                {
                    return new BuisnessLogicErrorResult("AccessToken invalid or expired");
                }

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var sessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(accessToken);

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid))
                {
                    return new BuisnessLogicErrorResult("Invalid token structure", 400);
                }

                _logger.LogInformation("Access token validation successful for {AccessToken}", accessToken);
                return new BuisnessLogicSuccessResult("Access token validation successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Access token validation failed");
                return new BuisnessLogicErrorResult("Access token validation failed", 500);
            }
        }

        public async Task<IBuisnessLogicResult> IsAccessTokenValidAsync(LogoutCommand request)
        {
            try
            {
                var result = await IsAccessTokenValidAsync(request.AccessToken);

                if (result is BuisnessLogicErrorResult errorResult)
                {
                    _logger.LogError("Access token validation failed: {Message}", errorResult.Message);
                    return errorResult;
                }

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(request.AccessToken);
                var sessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(request.AccessToken);

                request.UserUuid = userUuid;
                request.SessionUuid = sessionUuid;

                _logger.LogInformation("Access token validation successful for {AccessToken}", request.AccessToken);
                return new BuisnessLogicSuccessResult("Access token validation successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Access token validation failed for logout command");
                return new BuisnessLogicErrorResult("Access token validation failed", 500);
            }
        }

        public async Task<IBuisnessLogicResult> IsAccessTokenValidAsync(RefreshTokenCommand request)
        {
            try
            {
                var result = await IsAccessTokenValidAsync(request.AccessToken);
                if (result is BuisnessLogicErrorResult errorResult)
                {
                    _logger.LogError("Access token validation failed: {Message}", errorResult.Message);
                    return errorResult;
                }

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(request.AccessToken);
                var sessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(request.AccessToken);
                request.UserUuid = userUuid;
                request.SessionUuid = sessionUuid;

                _logger.LogInformation("Access token validation successful for {AccessToken}", request.AccessToken);
                return new BuisnessLogicSuccessResult("Access token validation successful", 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("Refresh token işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistSeesionTokensAsync(string? userUuid, string? sessionUuid)
        {
            try
            {
                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid))
                {
                    _logger.LogError("User UUID or Session UUID is null or empty");
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are required", 400);
                }

                var revokeResult = await _sessionJwtService.RevokeTokensAsync(userUuid, sessionUuid);
                if (!revokeResult)
                {
                    _logger.LogError("Failed to revoke tokens for user {UserUuid} and session {SessionUuid}", userUuid, sessionUuid);
                    return new BuisnessLogicErrorResult("Failed to revoke tokens", 500);
                }

                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error blacklisting access token for user {UserUuid} and session {SessionUuid}", userUuid, sessionUuid);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistSessionsExcludedByOneAsync(string? userUuid, string? sessionUuid, string? excludedSessionUuid)
        {
            try
            {
                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid) || string.IsNullOrEmpty(excludedSessionUuid))
                {
                    _logger.LogError("User UUID or Session UUID is null or empty");
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are required", 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensExcluededByOne(userUuid, excludedSessionUuid);

                if (!revokeResult)
                {
                    _logger.LogError("Failed to revoke tokens for user {UserUuid} excluding session {ExcludedSessionUuid}", userUuid, excludedSessionUuid);
                    return new BuisnessLogicErrorResult("Failed to revoke tokens", 500);
                }

                _logger.LogInformation("Access token blacklisted successfully for user {UserUuid} excluding session {ExcludedSessionUuid}", userUuid, excludedSessionUuid);
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error blacklisting access token for user {UserUuid} and session {SessionUuid}", userUuid, sessionUuid);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string? userUuid)
        {
            try
            {
                _logger.LogInformation("Blacklisting all session tokens for user {UserUuid}", userUuid);
                if (string.IsNullOrEmpty(userUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID is required", 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensAsync(userUuid);
                _logger.LogInformation("All session tokens blacklisted successfully for user {UserUuid}", userUuid);

                if (!revokeResult)
                {
                    _logger.LogError("Failed to revoke all session tokens for user {UserUuid}", userUuid);
                    return new BuisnessLogicErrorResult("Failed to revoke all session tokens", 500);
                }
                return new BuisnessLogicSuccessResult("All session tokens blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error blacklisting all session tokens for user {UserUuid}", userUuid);
                return new BuisnessLogicErrorResult("Error blacklisting all session tokens", 500);
            }
        }

        public async Task<IBuisnessLogicDataResult<RefreshTokenCommand>> RefreshAccessToken(RefreshTokenCommand request)
        {
            try
            {
                _logger.LogInformation("Refresh token işlemi başlatıldı. Refresh Token: {RefreshToken}", request.RefreshToken);

                if (string.IsNullOrEmpty(request.RefreshToken) || string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return new BuisnessLogicErrorDataResult<RefreshTokenCommand>(
                        null, "Refresh token is required", 400);
                }

                if (string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    return new BuisnessLogicErrorDataResult<RefreshTokenCommand>(
                        null, "Access token is required", 400);
                }

                
                var refreshResult = await _sessionJwtService.RefreshAccessTokenAsync(request.RefreshToken);
                if (refreshResult == null)
                {
                    _logger.LogError("Refresh token işlemi başarısız oldu. Refresh Token: {RefreshToken}", request.RefreshToken);
                    return new BuisnessLogicErrorDataResult<RefreshTokenCommand>(
                        null, "Refresh token işlemi başarısız oldu", 500);
                }

                RefreshTokenCommand rewponseData = new RefreshTokenCommand
                {
                    AccessToken = refreshResult.AccessToken,
                    RefreshToken = refreshResult.RefreshToken,
                    UserUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(refreshResult.AccessToken),
                    SessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(refreshResult.AccessToken)
                };

                return new BuisnessLogicSuccessDataResult<RefreshTokenCommand>(
                    data: rewponseData,
                    message: "Refresh token işlemi başarılı",
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh token işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorDataResult<RefreshTokenCommand>(
                    null, "Refresh token işlemi sırasında hata oluştu", 500);
            }
        }


    }
}
