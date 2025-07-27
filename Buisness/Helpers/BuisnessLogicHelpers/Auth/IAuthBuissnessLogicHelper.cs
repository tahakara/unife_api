using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Helpers.Common.HelperEnums;
using Buisness.Services.EntityRepositoryServices.Base;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Services.UtilityServices.Base.EmailServices;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Core.Enums;
using Core.Utilities.BuisnessLogic.Base;
using Core.Utilities.BuisnessLogic.BuisnesLogicMessages;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Utilities.OTPUtilities;
using Core.Utilities.OTPUtilities.Base;
using Core.Utilities.PasswordUtilities.Base;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Buisness.Helpers.BuisnessLogicHelpers.Auth
{
    /// <summary>
    /// Provides business logic helper methods for authentication, authorization, session management, 
    /// password management, OTP verification, and security event logging.
    /// </summary>
    public interface IAuthBuisnessLogicHelper : IBuisnessLogicHelper, IServiceManagerBase
    {
        /// <summary>
        /// Validates the provided access token for authenticity and expiration.
        /// </summary>
        /// <param name="accessToken">The access token to validate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken);

        /// <summary>
        /// Blacklists sessions based on the provided access token and blacklist mode.
        /// </summary>
        /// <param name="accessToken">The access token identifying the session(s).</param>
        /// <param name="mode">The blacklist mode (single, all, or all except one).</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> BlacklistSessionsAsync(string accessToken, BlacklistMode mode);

        #region Refresh Token

        /// <summary>
        /// Validates the provided refresh token and populates the response DTO if valid.
        /// </summary>
        /// <param name="refreshTokenRequestDto">The refresh token request DTO.</param>
        /// <param name="refreshTokenResponseDto">The refresh token response DTO to populate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto);

        /// <summary>
        /// Refreshes the access token using the provided refresh token response DTO.
        /// </summary>
        /// <param name="refreshTokenResponseDto">The refresh token response DTO containing necessary information.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> RefreshAccessTokenAsync(RefreshTokenResponseDto refreshTokenResponseDto);

        #endregion

        #region SignUp

        /// <summary>
        /// Checks the sign-up credentials and creates a new user if valid.
        /// </summary>
        /// <param name="signUpRequestDto">The sign-up request DTO.</param>
        /// <param name="signUpResponseDto">The sign-up response DTO to populate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CheckAndCreateSignUpCredentialsAsync(SignUpRequestDto signUpRequestDto, SignUpResponseDto signUpResponseDto);

        #endregion

        #region SignIn / Resend OTP

        /// <summary>
        /// Prevents brute force attacks during sign-in by tracking failed attempts.
        /// </summary>
        /// <param name="signInRequestDto">The sign-in request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> PreventSignInBruteForceAsync(SignInRequestDto signInRequestDto);

        /// <summary>
        /// Checks if the user's session count has exceeded the allowed limit.
        /// </summary>
        /// <param name="signInResponseDto">The sign-in response DTO containing user information.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CheckUserSessionCountExceededAsync(SignInResponseDto signInResponseDto);

        #region Resend OTP

        /// <summary>
        /// Checks the sign-in credentials for validity and populates the response DTO.
        /// </summary>
        /// <param name="signInRequestDto">The sign-in request DTO.</param>
        /// <param name="signInResponseDto">The sign-in response DTO to populate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CheckSignInCredentialsAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto);

        /// <summary>
        /// Sends a sign-in OTP to the user based on the provided request and response DTOs.
        /// </summary>
        /// <param name="signInRequestDto">The sign-in request DTO.</param>
        /// <param name="signInResponseDto">The sign-in response DTO to populate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> SendSignInOTPAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto);

        /// <summary>
        /// Revokes any old OTP codes for the user to ensure only the latest OTP is valid.
        /// </summary>
        /// <param name="signInRequestDto">The sign-in request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> RevokeOldOTPAsync(SignInRequestDto signInRequestDto);

        #endregion

        #endregion

        #region Verify OTP

        /// <summary>
        /// Completes the sign-in process after successful OTP verification and sends notification.
        /// </summary>
        /// <param name="verifyOTPRequestDto">The OTP verification request DTO.</param>
        /// <param name="httpContext">The HTTP context for request details.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> SignInCompletedAsync(VerifyOTPRequestDto verifyOTPRequestDto, HttpContext httpContext);

        /// <summary>
        /// Checks the validity of the provided OTP and populates the response DTO.
        /// </summary>
        /// <param name="verifyOTPRequestDto">The OTP verification request DTO.</param>
        /// <param name="verifyOTPResponseDto">The OTP verification response DTO to populate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CheckVerifyOTPAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto);

        /// <summary>
        /// Creates a new session for the user after successful OTP verification.
        /// </summary>
        /// <param name="verifyOTPRequestDto">The OTP verification request DTO.</param>
        /// <param name="verifyOTPResponseDto">The OTP verification response DTO to populate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CreateSessionAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto);

        /// <summary>
        /// Revokes the brute force protection token for sign-in attempts for the specified user.
        /// </summary>
        /// <param name="userTypeId">The user type ID.</param>
        /// <param name="userUuid">The user's unique identifier.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> RevokeSignInBruteForceTokenAsync(byte userTypeId, Guid userUuid);

        #endregion

        #region Change Password

        /// <summary>
        /// Validates the provided password for the user identified by the access token.
        /// </summary>
        /// <param name="accessToken">The access token identifying the user.</param>
        /// <param name="password">The password to validate.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> ValidatePasswordAsync(string accessToken, string password);

        /// <summary>
        /// Changes the user's password after validating the old password.
        /// </summary>
        /// <param name="accessToken">The access token identifying the user.</param>
        /// <param name="oldPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> ChangePasswordAsync(string accessToken, string oldPassword, string newPassword);

        #endregion

        #region Forgot Password

        /// <summary>
        /// Checks the credentials provided for the forgot password process.
        /// </summary>
        /// <param name="forgotPasswordRequestDto">The forgot password request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CheckForgotPasswordCredentialsAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);

        /// <summary>
        /// Prevents brute force attacks during the forgot password process.
        /// </summary>
        /// <param name="forgotPasswordRequestDto">The forgot password request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> PreventForgotBruteForceAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);

        /// <summary>
        /// Sends a recovery notification (e.g., email) for the forgot password process.
        /// </summary>
        /// <param name="forgotPasswordRequestDto">The forgot password request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> SendRecoveryNotificationAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);

        /// <summary>
        /// Checks the validity of the provided recovery token for password reset.
        /// </summary>
        /// <param name="forgotPasswordRecoveryTokenRequestDto">The forgot password recovery token request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> CheckRecoveryToken(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto);

        /// <summary>
        /// Resets the user's password using the provided recovery token and new password.
        /// </summary>
        /// <param name="forgotPasswordRecoveryTokenRequestDto">The forgot password recovery token request DTO.</param>
        /// <returns>A business logic result indicating the outcome.</returns>
        Task<IBuisnessLogicResult> ResetUserPasswordAsync(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto);

        #endregion

        #region Security Events

        /// <summary>
        /// Adds a security event record to the database.
        /// </summary>
        /// <param name="httpContext">The HTTP context for retrieving request details.</param>
        /// <param name="eventTypeGuidKey">The key for the security event type GUID.</param>
        /// <param name="methodName">The name of the method triggering the event.</param>
        /// <param name="description">
        /// The event description text.  
        /// Only the first 1000 characters will be stored;  
        /// if a longer string is provided, it will be automatically truncated.
        /// </param>
        /// <param name="userGuid">The unique identifier of the user related to the event.</param>
        /// <param name="userTypeId">The ID of the user type related to the event.</param>
        /// <param name="accessToken">The access token, if available, for resolving user information.</param>
        /// <param name="isEventSuccess">Indicates if the event was successful.</param>
        /// <param name="failureMessage">A message describing the failure, if any.</param>
        /// <param name="additionalData">Additional data related to the security event.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBuisnessLogicResult> AddSecurityEventRecordAsync(
            HttpContext? httpContext,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            Guid? userGuid = null,
            UserTypeId userTypeId = UserTypeId._,
            string? accessToken = null,
            bool? isEventSuccess = null,
            string? failureMessage = null,
            Dictionary<string, object>? additionalData = null);

        #endregion
    }

    public class AuthBuisnessLogicHelper : ServiceManagerBase, IAuthBuisnessLogicHelper
    {
        private readonly ISessionJwtService _sessionJwtService;
        private readonly IOTPCodeService _OTPCodeService;
        private readonly IEmailService _emailService;
        private readonly IPasswordUtility _passwordUtility;
        private readonly IOTPUtilitiy _otpUtilitiy;
        private readonly IAdminService _adminService;
        private readonly IStaffService _staffService;
        private readonly IStudentService _studentService;
        private readonly ISecurityEventTypeService _securityEventTypeService;
        private readonly ISecurityEventService _securityEventService;

        public AuthBuisnessLogicHelper(
            ISessionJwtService sessionJwtService,
            IOTPCodeService OTPCodeService,
            IEmailService emailService,
            IPasswordUtility passwordUtility,
            IOTPUtilitiy otpUtilitiy,
            IAdminService adminService,
            IStaffService staffService,
            IStudentService studentService,
            ISecurityEventTypeService securityEventTypeService,
            ISecurityEventService securityEventService,
            IMapper mapper,
            ILogger<AuthBuisnessLogicHelper> logger,
            IServiceProvider serviceProvider) : base(mapper, logger, serviceProvider)
        {
            _sessionJwtService = sessionJwtService;
            _OTPCodeService = OTPCodeService;
            _emailService = emailService;
            _passwordUtility = passwordUtility;
            _otpUtilitiy = otpUtilitiy;
            _adminService = adminService;
            _staffService = staffService;
            _studentService = studentService;
            _securityEventTypeService = securityEventTypeService;
            _securityEventService = securityEventService;
        }

        public async Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("IsAccessTokenValidAsync Started");

                bool isValid = await _sessionJwtService.ValidateTokenAsync(accessToken);
                if (!isValid)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("AccessToken"));
                }

                await LogDebugAsync("IsAccessTokenValidAsync Completed");
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Valid("AccessToken"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("IsAccessTokenValidAsync Excepted", ex);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Validating AccessToken"), 500);
            }
        }

        #region Blacklist Methods
        public async Task<IBuisnessLogicResult> BlacklistSessionsAsync(string accessToken, BlacklistMode mode)
        {
            return mode switch
            {
                BlacklistMode.Single => await BlacklistSingleSessionTokenAsync(accessToken),
                BlacklistMode.All => await BlacklistAllSessionTokensByUserAsync(accessToken),
                BlacklistMode.AllExceptOne => await BlacklistAllSessionsExceptOneAsync(accessToken),
                _ => new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("BalcklistMode"), 400)
            };
        }
        #region Private Methods
        private async Task<IBuisnessLogicResult> BlacklistSingleSessionTokenAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("BlacklistSingleSessionTokenAsync Started");

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var sessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(accessToken);
                var userTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid) || string.IsNullOrEmpty(userTypeId))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound(new[] { "UserUuid", "SessionUuid" }, "AccessToken"), 400);
                }

                var revokeResult = await _sessionJwtService.RevokeTokensAsync(userUuid, sessionUuid, userTypeId);
                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.RevokcationFailed("Tokens"), 500);
                }

                await LogDebugAsync("BlacklistSingleSessionTokenAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid, SessionUuid = sessionUuid });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Blacklisted("AccessToken"), 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistSingleSessionTokenAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Blacklisting Token"), 500);
            }
        }

        private async Task<IBuisnessLogicResult> BlacklistAllSessionsExceptOneAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("BlacklistAllSessionsExceptOneAsync Started", accessToken);

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var currentSessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(accessToken);

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(currentSessionUuid))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound(new[] { "UserUuid", "SessionUuid" }, "AccessToken"), 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensExcluededByOne(userUuid, currentSessionUuid);

                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.RevokcationFailed("Tokens"), 500);
                }

                await LogDebugAsync("BlacklistAllSessionsExceptOneAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid, CurrentSessionUuid = currentSessionUuid });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Blacklisted("AccessToken"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistAllSessionsExceptOneAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Blacklisting Tokens"), 500);
            }
        }

        private async Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("BlacklistAllSessionTokensByUserAsync Started", accessToken);

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                if (string.IsNullOrEmpty(userUuid))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserUuid", "AccessToken"), 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensAsync(userUuid);

                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.RevokcationFailed("Tokens"), 500);
                }

                await LogDebugAsync("BlacklistAllSessionTokensByUserAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Revoked("All Tokens"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistAllSessionTokensByUserAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Blacklisting Tokens"), 500);
            }
        }
        #endregion

        #endregion


        #region Referesh Token Methods
        public async Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto)
        {
            try
            {
                await LogDebugAsync("IsRefreshTokenValidAsync Started", refreshTokenRequestDto.RefreshToken);

                await ValidateAsync(refreshTokenRequestDto);

                if (string.IsNullOrEmpty(refreshTokenRequestDto.RefreshToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.RefreshToken))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Required("RefreshToken"), 400);
                }

                if (string.IsNullOrEmpty(refreshTokenRequestDto.AccessToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.AccessToken))
                {
                    var postfixSearchKey = await _sessionJwtService.GetRefreshTokenKeyByRefreshTokenPostfixAsync(refreshTokenRequestDto.RefreshToken);

                    if (string.IsNullOrEmpty(postfixSearchKey))
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("RefreshToken"), 400);
                    }

                    JsonElement? postfixSearchResult = await _sessionJwtService.GetRefreshTokenValue(postfixSearchKey);
                    if (postfixSearchKey == null)
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("RefreshToken"), 400);
                    }

                    if (!postfixSearchResult.HasValue || !postfixSearchResult.Value.TryGetProperty("UserUuid", out var userUuid) || !postfixSearchResult.Value.TryGetProperty("SessionUuid", out var sessionUuid) || !postfixSearchResult.Value.TryGetProperty("UserTypeId", out var userTypeId))
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("RefreshToken"), 400);
                    }

                    refreshTokenResponseDto.UserUuid = userUuid.GetString() ?? string.Empty;
                    refreshTokenResponseDto.SessionUuid = sessionUuid.GetString() ?? string.Empty;
                    refreshTokenResponseDto.RefreshToken = refreshTokenRequestDto.RefreshToken;
                    refreshTokenResponseDto.UserTypeId = byte.TryParse(userTypeId.GetString(), out var parsedUserTypeId) ? parsedUserTypeId : (byte)0;

                    return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Valid("RefreshToken"), 200);

                }

                refreshTokenResponseDto.UserUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(refreshTokenRequestDto.AccessToken);
                refreshTokenResponseDto.SessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(refreshTokenRequestDto.AccessToken);
                refreshTokenResponseDto.RefreshToken = refreshTokenRequestDto.RefreshToken;
                if (string.IsNullOrEmpty(refreshTokenResponseDto.UserUuid) || string.IsNullOrEmpty(refreshTokenResponseDto.SessionUuid))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound(new[] { "UserUuid", "SessionUuid"}, "AccessToken"), 400);
                }

                await LogDebugAsync("IsRefreshTokenValidAsync Completed", new
                {
                    AccessToken = refreshTokenRequestDto.AccessToken,
                    UserUuid = refreshTokenResponseDto.UserUuid,
                    SessionUuid = refreshTokenResponseDto.SessionUuid,
                    RefreshToken = refreshTokenResponseDto.RefreshToken
                });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Valid("RefreshToken"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("IsRefreshTokenValidAsync Excepted", ex, refreshTokenRequestDto.RefreshToken);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Validationd RefreshToken"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> RefreshAccessTokenAsync(RefreshTokenResponseDto refreshTokenResponseDto)
        {
            try
            {
                await LogDebugAsync("RefreshAccessTokenAsync Started", refreshTokenResponseDto.RefreshToken);

                var refreshResult = await _sessionJwtService.RefreshAccessTokenAsync(refreshTokenResponseDto.UserTypeId.ToString(), refreshTokenResponseDto.UserUuid, refreshTokenResponseDto.SessionUuid, refreshTokenResponseDto.RefreshToken);
                if (refreshResult == null)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Failed("RefreshResult"), 500);
                }

                refreshTokenResponseDto.AccessToken = refreshResult.AccessToken;
                refreshTokenResponseDto.RefreshToken = refreshResult.RefreshToken;

                await LogDebugAsync("RefreshAccessTokenAsync Completed", new
                {
                    AccessToken = refreshTokenResponseDto.AccessToken,
                    RefreshToken = refreshTokenResponseDto.RefreshToken,
                    UserUuid = refreshTokenResponseDto.UserUuid,
                    SessionUuid = refreshTokenResponseDto.SessionUuid
                });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("RefreshToken"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("RefreshAccessTokenAsync Excepted", ex, refreshTokenResponseDto.RefreshToken);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Refreshing AccessToken"), 500);
            }
        }
        #endregion


        #region SignUp Methods
        public async Task<IBuisnessLogicResult> CheckAndCreateSignUpCredentialsAsync(SignUpRequestDto signUpRequestDto, SignUpResponseDto signUpResponseDto)
        {
            try
            {
                await LogDebugAsync("CheckSignUpCredentialsAsync Started", signUpRequestDto);

                switch (signUpRequestDto.UserTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        bool emailResult = await _adminService.IsEmailExistsAsync(signUpRequestDto.Email);
                        if (emailResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotAccepted("Email"), 400);
                        }

                        bool phoneNumberResult = await _adminService.IsPhoneNumberExistsAsync(signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);

                        if (phoneNumberResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotAccepted("PhoneNumber"), 400);
                        }

                        (byte[] hash, byte[] salt) = _passwordUtility.HashPassword(signUpRequestDto.Password);
                        Admin newAdmin = new Admin();

                        _mapper.Map(signUpRequestDto, newAdmin);

                        newAdmin.PasswordHash = hash;
                        newAdmin.PasswordSalt = salt;

                        newAdmin = await _adminService.CreateNewAdminAsync(newAdmin);

                        if (newAdmin == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotCreated("Admin"), 500);
                        }

                        _mapper.Map(newAdmin, signUpResponseDto);

                        return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("Admin"), 200);
                        break;

                    case (byte)UserTypeId.Staff:
                        bool staffEmailResult = await _staffService.IsEmailExistsAsync(signUpRequestDto.Email);
                        if (staffEmailResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotAccepted("Email"), 400);
                        }
                        bool staffPhoneNumberResult = await _staffService.IsPhoneNumberExistsAsync(signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);
                        if (staffPhoneNumberResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotAccepted("PhoneNumber"), 400);
                        }

                        (byte[] staffHash, byte[] staffSalt) = _passwordUtility.HashPassword(signUpRequestDto.Password);
                        Staff newStaff = new Staff();

                        _mapper.Map(signUpRequestDto, newStaff);
                        newStaff.PasswordHash = staffHash;
                        newStaff.PasswordSalt = staffSalt;

                        newStaff = await _staffService.CreateNewStaffAsync(newStaff);
                        if (newStaff == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotCreated("Staff"), 500);
                        }

                        _mapper.Map(newStaff, signUpResponseDto);

                        return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("Staff"), 200);
                        break;
                    case (byte)UserTypeId.Student:

                        bool studentEmailResult = await _studentService.IsEmailExistsAsync(signUpRequestDto.Email);
                        if (studentEmailResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotAccepted("Email"), 400);
                        }

                        bool studentPhoneNumberResult = await _studentService.IsPhoneNumberExistsAsync(signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);

                        if (studentPhoneNumberResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotAccepted("PhoneNumber"), 400);
                        }

                        (byte[] studentHash, byte[] studentSalt) = _passwordUtility.HashPassword(signUpRequestDto.Password);
                        Student newStudent = new Student();

                        _mapper.Map(signUpRequestDto, newStudent);
                        newStudent.PasswordHash = studentHash;
                        newStudent.PasswordSalt = studentSalt;

                        newStudent = await _studentService.CreateNewStudentAsync(newStudent);

                        if (newStudent == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotCreated("Student"), 500);
                        }

                        _mapper.Map(newStudent, signUpResponseDto);

                        return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("Student"), 200);
                        break;

                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                        break;
                }
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserTypeId"), 400);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckSignUpCredentialsAsync Excepted", ex, signUpRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking SignUp Credentials"), 500);
            }
        }
        #endregion


        #region SignIn / Resend OTP Methods
        public async Task<IBuisnessLogicResult> PreventSignInBruteForceAsync(SignInRequestDto signInRequestDto)
        {
            try
            {
                await LogDebugAsync("PreventSignInBruteForceAsync başladı");

                var key = _sessionJwtService.GenerateSignInOTPBruteForceProtectionKey(signInRequestDto.Email, signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);

                if (string.IsNullOrEmpty(key))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("BruteForceKey"), 400);
                }

                bool exists = await _sessionJwtService.IsSignInOTPBruteForceProtectionKeyExistsAsync(key);

                if (exists)
                {
                    int attempts = 0;
                    attempts = await _sessionJwtService.GetSignInOTPBruteForceProtectionAttemptsByKeyAsync(key);
                    if (attempts >= 5)
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.ToManyAttempts("SignIn"), 429);
                    }
                    await _sessionJwtService.IncrementSignInOTPBruteForceProtectionAttemptsAsync(key);

                    return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("BruteForceKeyProtection"), 200);
                }

                // Increment or set
                await _sessionJwtService.SetSignInOTPBruteForceProtectionKeyAsync(key);

                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("BruteForceKeyProtection"), 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PreventSignInBruteForceAsync hata");
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking Protection Keys"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> CheckUserSessionCountExceededAsync(SignInResponseDto signInResponseDto)
        {
            try
            {
                await LogDebugAsync("CheckUserSessionCountExceeded Started", signInResponseDto);

                int sessionCount = await _sessionJwtService.GetSessionCountByUserUuid(signInResponseDto.UserUuid.ToString());

                if (sessionCount >= 10) // Assuming 5 is the maximum allowed sessions
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.ExceedsLimit("User Session", 10), 403);
                }

                await LogDebugAsync("CheckUserSessionCountExceeded Completed", signInResponseDto);
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("Checked UserSessions"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckUserSessionCountExceeded Excepted", ex, signInResponseDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking User Sessions"), 500);
            }
        }

        #region Resend OTP Methods
        public async Task<IBuisnessLogicResult> CheckSignInCredentialsAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto)
        {
            try
            {
                await LogDebugAsync("CheckSignInCredentialsAsync Started", signInRequestDto);

                switch (signInRequestDto.UserTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        Admin? currentAdmin = new();

                        bool hasEmailLogin = !string.IsNullOrEmpty(signInRequestDto.Email) &&
                                                !string.IsNullOrEmpty(signInRequestDto.Password);

                        bool hasPhoneLogin = !string.IsNullOrEmpty(signInRequestDto.PhoneCountryCode) &&
                                             !string.IsNullOrEmpty(signInRequestDto.PhoneNumber) &&
                                             !string.IsNullOrEmpty(signInRequestDto.Password);


                        if (hasEmailLogin)
                        {
                            currentAdmin = await _adminService.GetAdminByEmailAsync(signInRequestDto.Email);
                        }
                        else if (hasPhoneLogin)
                        {
                            currentAdmin = await _adminService.GetAdminByPhoneNumberAsync(signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);
                        }
                        else { return new BuisnessLogicErrorResult(BuisnessLogicMessage.AtLeastOneRequired(new[] { "(Email & Password)", "(PhoneCountryCode & PhoneNumber & Password)" }), 400); }

                        if (currentAdmin == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Admin"), 404);
                        }

                        if (!_passwordUtility.VerifyPassword(signInRequestDto.Password, currentAdmin.PasswordHash, currentAdmin.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        signInRequestDto.UserUuid = currentAdmin.AdminUuid;

                        signInResponseDto.UserUuid = currentAdmin.AdminUuid;
                        signInResponseDto.UserTypeId = (byte)UserTypeId.Admin;
                        break;

                    case (byte)UserTypeId.Staff:
                        Staff? currentStaff = new();

                        if (string.IsNullOrEmpty(signInRequestDto.Email) && string.IsNullOrEmpty(signInRequestDto.Password))
                        {
                            currentStaff = await _staffService.GetStaffByEmailAsync(signInRequestDto.Email);
                        }
                        else if ((string.IsNullOrEmpty(signInRequestDto.PhoneCountryCode) && string.IsNullOrEmpty(signInRequestDto.PhoneNumber)) &&
                            string.IsNullOrEmpty(signInRequestDto.Password))
                        {
                            currentStaff = await _staffService.GetStaffByPhoneNumberAsync(signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);
                        }
                        else { return new BuisnessLogicErrorResult(BuisnessLogicMessage.AtLeastOneRequired(new[] { "(Email & Password)", "(PhoneCountryCode & PhoneNumber & Password)" }), 400); }

                        if (currentStaff == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Staff"), 404);
                        }

                        if (!_passwordUtility.VerifyPassword(signInRequestDto.Password, currentStaff.PasswordHash, currentStaff.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }

                        signInRequestDto.UserUuid = currentStaff.StaffUuid;

                        signInResponseDto.UserUuid = currentStaff.StaffUuid;
                        signInResponseDto.UserTypeId = (byte)UserTypeId.Staff;
                        break;

                    case (byte)UserTypeId.Student:
                        Student? currentStudent = new();

                        if (string.IsNullOrEmpty(signInRequestDto.Email) && string.IsNullOrEmpty(signInRequestDto.Password))
                        {
                            currentStudent = await _studentService.GetStudentByEmailAsync(signInRequestDto.Email);
                        }
                        else if ((string.IsNullOrEmpty(signInRequestDto.PhoneCountryCode) && string.IsNullOrEmpty(signInRequestDto.PhoneNumber)) &&
                            string.IsNullOrEmpty(signInRequestDto.Password))
                        {
                            currentStudent = await _studentService.GetStudentByPhoneNumberAsync(signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);
                        }
                        else { return new BuisnessLogicErrorResult(BuisnessLogicMessage.AtLeastOneRequired(new[] { "(Email & Password)", "(PhoneCountryCode & PhoneNumber & Password)" }), 400); }

                        if (currentStudent == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Student"), 404);
                        }

                        if (!_passwordUtility.VerifyPassword(signInRequestDto.Password, currentStudent.PasswordHash, currentStudent.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }

                        signInRequestDto.UserUuid = currentStudent.StudentUuid;

                        signInResponseDto.UserUuid = currentStudent.StudentUuid;
                        signInResponseDto.UserTypeId = (byte)UserTypeId.Student;
                        break;

                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                        break;
                }

                await LogDebugAsync("CheckSignInCredentialsAsync Completed", signInRequestDto);
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Valid("Credentials"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckSignInCredentialsAsync Excepted", ex, signInRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking SignIn Credentials"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> SendSignInOTPAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto)
        {
            try
            {
                await LogDebugAsync("SendSignInOTPAsync Started", signInRequestDto);

                signInRequestDto.SessionUuid = Guid.NewGuid();
                signInRequestDto.OtpCode = _otpUtilitiy.GenerateOTP();
                signInRequestDto.OtpTypeId = (byte)OTPTypeId.Email; // Default to Email, can be changed based on user preference

                // Response Dto
                signInResponseDto.SessionUuid = signInRequestDto.SessionUuid;
                signInResponseDto.OtpTypeId = signInRequestDto.OtpTypeId;

                switch (signInRequestDto.OtpTypeId)
                {
                    case (byte)OTPTypeId.Email:

                        bool result = await _OTPCodeService.SetCodeAsync(
                            signInRequestDto.SessionUuid.ToString(),
                            signInRequestDto.UserUuid.ToString(),
                            signInRequestDto.OtpTypeId.ToString(),
                            signInRequestDto.OtpCode);

                        if (!result)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Failed("Setting","OTP"), 500);
                        }

                        bool mailResult = await _emailService.SendSignInOtpCode(signInRequestDto.Email, signInRequestDto.OtpCode);
                        if (!mailResult)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Failed("Sending", "OTP Via Email"), 500);
                        }
                        break;

                    //case (byte)OTPTypeId.Sms:
                    //    bool smsResult = await _emailService.SendSignInOtpCode(signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber, signInRequestDto.OtpCode);
                    //    break;

                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("OTPType"), 400);
                        break;
                }

                await LogDebugAsync("SendSignInOTP Completed", new
                {
                    UserType = signInRequestDto.UserTypeId,
                    Email = signInRequestDto.Email,
                    PhoneCountryCode = signInRequestDto.PhoneCountryCode,
                    PhoneNumber = signInRequestDto.PhoneNumber,
                    SessionUuid = signInRequestDto.SessionUuid,
                    OtpTypeId = signInRequestDto.OtpTypeId,
                    OtpCode = signInRequestDto.OtpCode
                });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("SignIn OTP was Sended"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("SendSignInOTPAsync Excepted", ex, signInRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Sending SignIn OTP"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> RevokeOldOTPAsync(SignInRequestDto signInRequestDto)
        {
            try
            {
                await LogDebugAsync("RevokeOldOTP Started", signInRequestDto);

                bool isRevoked = await _OTPCodeService.RevokeCodeByUserUuid(signInRequestDto.UserUuid.ToString());
                if (!isRevoked)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.RevokcationFailed("Old OTP"), 500);
                }

                await LogDebugAsync("RevokeOldOTP Completed", new
                {
                    UserType = signInRequestDto.UserTypeId,
                    SessionUuid = signInRequestDto.SessionUuid,
                    OtpTypeId = signInRequestDto.OtpTypeId
                });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("Revoked Old OTPs"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("RevokeOldOTPAsync Excepted", ex, signInRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Revoking Old OTPs"), 500);
            }
        }
        #endregion

        #endregion


        #region Verify OTP Methods

        public async Task<IBuisnessLogicResult> CheckVerifyOTPAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto)
        {
            try
            {
                await LogDebugAsync("CheckVerifyOTPAsync Started", verifyOTPRequestDto);

                // Is OTP code exist?
                var otpRecord = await _OTPCodeService.GetCodeExistBySessionUuidAndUserUuidAsync(
                    verifyOTPRequestDto.SessionUuid.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.OtpTypeId.ToString());

                if (otpRecord == null)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("OTP"), 400);
                }

                // Is OTP code already used?
                int attempts = 0;
                int ttlSeconds = 180; // default

                if (otpRecord is JsonElement json)
                {
                    if (json.TryGetProperty("Attemps", out var att))
                        attempts = att.GetInt32();

                    if (json.TryGetProperty("ExpiresAt", out var exp))
                        if (DateTime.TryParse(exp.GetString(), out var expiresAt))
                            ttlSeconds = Math.Max((int)(expiresAt - DateTime.UtcNow).TotalSeconds, 1);
                }
                else
                {
                    var propAtt = otpRecord.GetType().GetProperty("Attemps");
                    if (propAtt != null)
                        attempts = Convert.ToInt32(propAtt.GetValue(otpRecord));

                    var propExp = otpRecord.GetType().GetProperty("ExpiresAt");
                    if (propExp?.GetValue(otpRecord) is DateTime expiresAt)
                        ttlSeconds = Math.Max((int)(expiresAt - DateTime.UtcNow).TotalSeconds, 1);
                }

                // Is OTP code expired?
                if (attempts >= 5)
                {
                    await _OTPCodeService.RemoveCodeAsync(
                        verifyOTPRequestDto.SessionUuid.ToString(),
                        verifyOTPRequestDto.UserUuid.ToString(),
                        verifyOTPRequestDto.OtpTypeId.ToString(),
                        null);

                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.ToManyAttempts("OTP"), 400);
                }

                // Is OTP code provided?
                if (string.IsNullOrWhiteSpace(verifyOTPRequestDto.OtpCode))
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Required("OTP"), 400);

                bool isValid = verifyOTPRequestDto.OtpCode == (
                    otpRecord is JsonElement je && je.TryGetProperty("OtpCode", out var oc)
                        ? oc.GetString()
                        : otpRecord.GetType().GetProperty("OtpCode")?.GetValue(otpRecord)?.ToString()
                );

                if (!isValid)
                {
                    string? storedOtpCode = null;

                    if (otpRecord is JsonElement jsonElement && jsonElement.TryGetProperty("OtpCode", out var otpCodeProp))
                    {
                        storedOtpCode = otpCodeProp.GetString();
                    }
                    else
                    {
                        storedOtpCode = otpRecord.GetType().GetProperty("OtpCode")?.GetValue(otpRecord)?.ToString();
                    }

                    bool updated = await _OTPCodeService.SetCodeAsync(
                        verifyOTPRequestDto.SessionUuid.ToString(),
                        verifyOTPRequestDto.UserUuid.ToString(),
                        verifyOTPRequestDto.OtpTypeId.ToString(),
                        storedOtpCode,
                        attempts,
                        ttlSeconds > 0 ? TimeSpan.FromSeconds(ttlSeconds) : TimeSpan.FromMinutes(3)
                    );

                    if (!updated)
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Failed("OTP"), 500);

                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("OTP"), 400);
                }

                // Is OTP code valid?
                bool removed = await _OTPCodeService.RemoveCodeAsync(
                    verifyOTPRequestDto.SessionUuid.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.OtpTypeId.ToString(),
                    verifyOTPRequestDto.OtpCode);

                if (!removed)
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Failed("OTP"), 500);

                // Is OTP code verified?
                verifyOTPResponseDto.SessionUuid = verifyOTPRequestDto.SessionUuid;
                verifyOTPResponseDto.OtpTypeId = verifyOTPRequestDto.OtpTypeId;
                verifyOTPResponseDto.UserUuid = verifyOTPRequestDto.UserUuid;

                await LogDebugAsync("CheckVerifyOTP Completed", new
                {
                    verifyOTPRequestDto.UserUuid,
                    verifyOTPRequestDto.SessionUuid,
                    verifyOTPRequestDto.OtpTypeId
                });

                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("OTP"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckVerifyOTPAsync Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking OTP"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> CreateSessionAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto)
        {
            try
            {
                await LogDebugAsync("CreateSessionAsync Started", verifyOTPRequestDto);

                string newAccessTOken = await _sessionJwtService.GenerateAccessTokenAsync(
                    verifyOTPRequestDto.UserTypeId.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.SessionUuid.ToString());

                string newRefreshToken = await _sessionJwtService.GenerateRefreshTokenAsync(
                    verifyOTPRequestDto.UserTypeId.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.SessionUuid.ToString());

                verifyOTPResponseDto.AccessToken = newAccessTOken;
                verifyOTPResponseDto.RefreshToken = newRefreshToken;

                await LogDebugAsync("CreateSessionAsync Completed", new
                {
                    UserType = verifyOTPRequestDto.UserTypeId,
                    SessionUuid = verifyOTPRequestDto.SessionUuid,
                    OtpTypeId = verifyOTPRequestDto.OtpTypeId,
                    AccessToken = newAccessTOken,
                    RefreshToken = newRefreshToken
                });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("Session"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CreateSessionAsync Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Creating Session"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> SignInCompletedAsync(VerifyOTPRequestDto verifyOTPRequestDto, HttpContext httpContext)
        {
            try
            {
                await LogDebugAsync("SiginCompleted Started", verifyOTPRequestDto);

                var ipAddress = httpContext.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
                var userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
                DateTime dateTime = DateTime.UtcNow;

                if (verifyOTPRequestDto == null || verifyOTPRequestDto.UserUuid == Guid.Empty)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Required("UserUuid"), 400);
                }

                switch (verifyOTPRequestDto.UserTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        Admin admin = await _adminService.GetByUuidAsync(verifyOTPRequestDto.UserUuid);
                        if (admin == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Admin"), 404);
                        }
                        await _emailService.SendSiginCompleteMail(ipAddress, userAgent, admin.Email, admin.FirstName, admin.MiddleName, admin.LastName);
                        break;

                    case (byte)UserTypeId.Staff:
                        Staff staff = await _staffService.GetByUuidAsync(verifyOTPRequestDto.UserUuid);
                        if (staff == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Staff"), 404);
                        }
                        await _emailService.SendSiginCompleteMail(ipAddress, userAgent, staff.Email, staff.FirstName, staff.MiddleName, staff.LastName);
                        break;
                    case (byte)UserTypeId.Student:
                        Student student = await _studentService.GetByUuidAsync(verifyOTPRequestDto.UserUuid);
                        if (student == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Student"), 404);
                        }
                        await _emailService.SendSiginCompleteMail(ipAddress, userAgent, student.Email, student.FirstName, student.MiddleName, student.LastName);
                        break;
                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                        break;
                }
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("SignIn Completed"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("SiginCompleted Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Completing SignIn"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> RevokeSignInBruteForceTokenAsync(byte userTypeId, Guid userUuid)
        {
            try
            {
                await LogDebugAsync("RevokeSignInBruteForceTokenAsync Started", new { UserTypeId = userTypeId, UserUuid = userUuid });

                string userEmial = string.Empty;
                string userPhoneNumber = string.Empty;
                string userPhoneCode = string.Empty;

                switch (userTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        var admin = await _adminService.GetByUuidAsync(userUuid);
                        if (admin == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Admin"), 404);
                        }
                        userEmial = admin.Email;
                        userPhoneNumber = admin.PhoneNumber;
                        userPhoneCode = admin.PhoneCountryCode;
                        break;
                    case (byte)UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(userUuid);
                        if (staff == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Staff"), 404);
                        }
                        userEmial = staff.Email;
                        userPhoneNumber = staff.PhoneNumber;
                        userPhoneCode = staff.PhoneCountryCode;
                        break;
                    case (byte)UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(userUuid);
                        if (student == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Student"), 404);
                        }
                        userEmial = student.Email;
                        userPhoneNumber = student.PhoneNumber;
                        userPhoneCode = student.PhoneCountryCode;
                        break;
                    default:
                        await LogErrorAsync("RevokeSignInBruteForceTokenAsync Invalid User Type", null, new { UserTypeId = userTypeId, UserUuid = userUuid });
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                        break;
                }

                if (string.IsNullOrEmpty(userEmial) && (string.IsNullOrEmpty(userPhoneNumber) || string.IsNullOrEmpty(userPhoneCode)))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Required(new[] { "Email", "PhoneNumber" }), 400);
                }

                var emailBruteForceKey = _sessionJwtService.GenerateSignInOTPBruteForceProtectionKey(email: userUuid.ToString(), phoneCode: null, phoneNumber: null);
                var phoneBruteForceKey = _sessionJwtService.GenerateSignInOTPBruteForceProtectionKey(email: null, phoneCode: null, phoneNumber: userUuid.ToString());

                bool emailResult = await _sessionJwtService.RemoveSignInOTPBruteForceProtectionKeyAsync(emailBruteForceKey);
                bool phoneResult = await _sessionJwtService.RemoveSignInOTPBruteForceProtectionKeyAsync(phoneBruteForceKey);

                await LogDebugAsync("RevokeSignInBruteForceTokenAsync Completed");
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("Revoked SignIn Protection Keys"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("RevokeSignInBruteForceTokenAsync Exception", ex, new { UserTypeId = userTypeId, UserUuid = userUuid });
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.Error("Revoking SignIn Protection Keys"), 500);
            }
        }

        #endregion


        #region Change Password Methods

        public async Task<IBuisnessLogicResult> ValidatePasswordAsync(string accessToken, string password)
        {
            try
            {
                await LogDebugAsync("ValidatePasswordAsync Started", new { AccessToken = accessToken, Password = password });

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var userTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);
                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(userTypeId))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound(new[] {"UserUuid", "SessionUuid"}, "AccessToken"), 400);
                }
                Guid uuid = Guid.Parse(userUuid);
                UserTypeId typeId = (UserTypeId)byte.Parse(userTypeId);
                switch (typeId)
                {
                    case UserTypeId.Admin:
                        var admin = await _adminService.GetByUuidAsync(uuid);
                        if (admin == null || !_passwordUtility.VerifyPassword(password, admin.PasswordHash, admin.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        break;
                    case UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(uuid);
                        if (staff == null || !_passwordUtility.VerifyPassword(password, staff.PasswordHash, staff.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        break;
                    case UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(uuid);
                        if (student == null || !_passwordUtility.VerifyPassword(password, student.PasswordHash, student.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        break;
                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                }
                await LogDebugAsync("ValidatePasswordAsync Completed", new { UserUuid = uuid, UserTypeId = typeId });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Valid("Password"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("ValidatePasswordAsync Excepted", ex, new { AccessToken = accessToken, Password = password });
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Validating Password"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> ChangePasswordAsync(string accessToken, string oldPassword, string newPassword)
        {
            try
            {
                await LogDebugAsync("ChangePasswordAsync Started", new { AccessToken = accessToken, OldPassword = oldPassword, NewPassword = newPassword });
                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                if (userUuid == null)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserUuid", "AccessToken"), 400);
                }
                var userTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);
                if (string.IsNullOrEmpty(userTypeId))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserTypeId", "AccessToken"), 400);
                }

                Guid uuid = Guid.Parse(userUuid);
                UserTypeId typeId = (UserTypeId)byte.Parse(userTypeId);
                switch (typeId)
                {
                    case UserTypeId.Admin:
                        var admin = await _adminService.GetByUuidAsync(uuid);
                        if (admin == null || !_passwordUtility.VerifyPassword(oldPassword, admin.PasswordHash, admin.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        (admin.PasswordSalt, admin.PasswordHash) = _passwordUtility.HashPassword(newPassword);
                        await _adminService.UpdateAdminAsync(admin);
                        break;
                    case UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(uuid);
                        if (staff == null || !_passwordUtility.VerifyPassword(oldPassword, staff.PasswordHash, staff.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        (staff.PasswordSalt, staff.PasswordHash) = _passwordUtility.HashPassword(newPassword);
                        await _staffService.UpdateStaffAsync(staff);
                        break;
                    case UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(uuid);
                        if (student == null || !_passwordUtility.VerifyPassword(oldPassword, student.PasswordHash, student.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Password"), 401);
                        }
                        (student.PasswordSalt, student.PasswordHash) = _passwordUtility.HashPassword(newPassword);
                        await _studentService.UpdateStudentAsync(student);
                        break;
                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                }

                await LogDebugAsync("ChangePasswordAsync Completed", new { UserUuid = uuid, UserTypeId = typeId });
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("Password Changed"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("ChangePasswordAsync Excepted", ex, new { AccessToken = accessToken, OldPassword = oldPassword, NewPassword = newPassword });
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Changing Password"), 500);
            }
        }

        #endregion



        #region Forgot Password Methods

        public async Task<IBuisnessLogicResult> CheckForgotPasswordCredentialsAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            try
            {
                await LogDebugAsync("CheckForgotPasswordCredentialsAsync Started", forgotPasswordRequestDto);


                bool isEmailProvided = !string.IsNullOrEmpty(forgotPasswordRequestDto.Email);
                bool isPhoneProvided =
                    !string.IsNullOrEmpty(forgotPasswordRequestDto.PhoneCountryCode) &&
                    !string.IsNullOrEmpty(forgotPasswordRequestDto.PhoneNumber);

                if (!isEmailProvided && !isPhoneProvided)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.AtLeastOneRequired(new[] {"Email", "PhoneNumber"}), 400);
                }


                switch (forgotPasswordRequestDto.UserTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        {
                            Admin? admin = null;

                            switch (forgotPasswordRequestDto.RecoveryMethodId)
                            {
                                case 1:
                                    admin = await _adminService.GetAdminByEmailAsync(forgotPasswordRequestDto.Email);
                                    break;
                                case 2:
                                    admin = await _adminService.GetAdminByPhoneNumberAsync(
                                        forgotPasswordRequestDto.PhoneCountryCode,
                                        forgotPasswordRequestDto.PhoneNumber);
                                    break;
                                default:
                                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Recovery Method"), 400);
                            }

                            if (admin == null)
                                return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Admin"), 404);

                            forgotPasswordRequestDto.UserUuid = admin.AdminUuid;
                        }
                        break;

                    case (byte)UserTypeId.Staff:
                        {
                            Staff? staff = null;

                            switch (forgotPasswordRequestDto.RecoveryMethodId)
                            {
                                case 1:
                                    staff = await _staffService.GetStaffByEmailAsync(forgotPasswordRequestDto.Email);
                                    break;
                                case 2:
                                    staff = await _staffService.GetStaffByPhoneNumberAsync(
                                        forgotPasswordRequestDto.PhoneCountryCode,
                                        forgotPasswordRequestDto.PhoneNumber);
                                    break;
                                default:
                                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Recovery Method"), 400);
                            }

                            if (staff == null)
                                return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Staff"), 404);

                            forgotPasswordRequestDto.UserUuid = staff.StaffUuid;
                        }
                        break;

                    case (byte)UserTypeId.Student:
                        {
                            Student? student = null;

                            switch (forgotPasswordRequestDto.RecoveryMethodId)
                            {
                                case 1:
                                    student = await _studentService.GetStudentByEmailAsync(forgotPasswordRequestDto.Email);
                                    break;
                                case 2:
                                    student = await _studentService.GetStudentByPhoneNumberAsync(
                                        forgotPasswordRequestDto.PhoneCountryCode,
                                        forgotPasswordRequestDto.PhoneNumber);
                                    break;
                                default:
                                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Recovery Method"), 400);
                            }

                            if (student == null)
                                return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Staff"), 404);

                            forgotPasswordRequestDto.UserUuid = student.StudentUuid;
                        }
                        break;

                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                }

                await LogDebugAsync("CheckForgotPasswordCredentialsAsync Completed", new
                {
                    forgotPasswordRequestDto.UserTypeId,
                    forgotPasswordRequestDto.Email,
                    forgotPasswordRequestDto.PhoneCountryCode,
                    forgotPasswordRequestDto.PhoneNumber
                });

                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Success("Recovery Code Sended"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckForgotPasswordCredentialsAsync Exception", ex, forgotPasswordRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking Recovery Code"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> PreventForgotBruteForceAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            try
            {
                await LogDebugAsync("PreventForgotBruteForceAsync Started", forgotPasswordRequestDto);

                string key = string.Empty;

                if (forgotPasswordRequestDto.RecoverySessionUuid == null)
                {
                    // takes only key 
                    key = await _sessionJwtService.GetForgotBruteForceProtectionKeyByUserUuidAsync(
                        forgotPasswordRequestDto.UserUuid.ToString());

                    if (!string.IsNullOrEmpty(key))
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.AlreadyExists("One Recovery Session"), 400);
                    }

                    forgotPasswordRequestDto.RecoveryToken = await _sessionJwtService.SetForgotBruteForceProtectionKeyAsync(
                        Guid.NewGuid().ToString(),
                        forgotPasswordRequestDto.UserTypeId.ToString(),
                        forgotPasswordRequestDto.UserUuid.ToString(),
                        forgotPasswordRequestDto.Email,
                        forgotPasswordRequestDto.PhoneCountryCode,
                        forgotPasswordRequestDto.PhoneNumber);

                    await LogDebugAsync("PreventForgotBruteForceAsync Completed", forgotPasswordRequestDto);
                    return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Created("Recovery Session"), 200);
                }

                // takes key and session uuid
                key = await _sessionJwtService.GetForgotBruteForceProtectionKeyByRecoverySessionUuidAsync(
                    forgotPasswordRequestDto.RecoverySessionUuid.ToString());
                if (!string.IsNullOrEmpty(key))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.AlreadyExists("One Recovery Session"), 400);
                }

                forgotPasswordRequestDto.RecoveryToken = await _sessionJwtService.SetForgotBruteForceProtectionKeyAsync(
                    Guid.NewGuid().ToString(),
                    forgotPasswordRequestDto.UserTypeId.ToString(),
                    forgotPasswordRequestDto.UserUuid.ToString(),
                    forgotPasswordRequestDto.Email,
                    forgotPasswordRequestDto.PhoneCountryCode,
                    forgotPasswordRequestDto.PhoneNumber);
                if (string.IsNullOrEmpty(forgotPasswordRequestDto.RecoveryToken))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotCreated("Recovery Session"), 500);
                }

                await LogDebugAsync("PreventForgotBruteForceAsync Completed", forgotPasswordRequestDto);
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Created("Recovery Session"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("PreventForgotBruteForceAsync Exception", ex, forgotPasswordRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking Recovery Session"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> SendRecoveryNotificationAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            try
            {
                await LogDebugAsync("SendRecoveryNotificationAsync Started", forgotPasswordRequestDto);

                if (string.IsNullOrEmpty(forgotPasswordRequestDto.RecoveryToken))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Recovery Session"), 400);
                }

                switch (forgotPasswordRequestDto.RecoveryMethodId)
                {
                    case 1:
                        // Email ile bildirim gönder
                        if (string.IsNullOrEmpty(forgotPasswordRequestDto.Email))
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Email"), 400);
                        }
                        bool result = await _emailService.SendForgotPasswordEmailAsync(
                            forgotPasswordRequestDto.Email,
                            forgotPasswordRequestDto.RecoveryToken);
                        if (!result)
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Email Sent"), 500);
                        return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("Email Sended"), 200);
                        break;

                    //    case 2:
                    //// Telefon ile bildirim gönder

                    //    await _smsService.SendForgotPasswordSmsAsync(
                    //        forgotPasswordRequestDto.PhoneCountryCode,
                    //        forgotPasswordRequestDto.PhoneNumber,
                    //        forgotPasswordRequestDto.recoveryToken,
                    //        forgotPasswordRequestDto.UserTypeId);
                    //    break;

                    default:
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("Recovery Method"), 400);
                        break;
                }
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.Success("Recovery"), 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("SendRecoveryNotificationAsync Exception", ex, forgotPasswordRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Sending Recovery Notification"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> CheckRecoveryToken(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto)
        {
            try
            {
                await LogDebugAsync("CheckRecoveryToken Started", forgotPasswordRecoveryTokenRequestDto);
                if (string.IsNullOrEmpty(forgotPasswordRecoveryTokenRequestDto.RecoveryToken))
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.Required("Recovery Token"), 400);
                }
                var isValid = await _sessionJwtService.IsForgotBruteForceProtectionKeyExistsAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);
                if (!isValid)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.InvalidOrExpired("Recovey Token"), 400);
                }
                string sessionUuid = await _sessionJwtService.GetForgotBruteForceProtectionSessionUuidByRecoveryTokenAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);
                string userUuid = await _sessionJwtService.GetForgotBruteForceProtectionUserUuidByRecoveryTokenAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);
                string userTypeId = await _sessionJwtService.GetForgotBruteForceProtectionUserTypeIdByRecoveryTokenAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);


                forgotPasswordRecoveryTokenRequestDto.RecoverySessionUuid = Guid.Parse(sessionUuid);
                forgotPasswordRecoveryTokenRequestDto.UserUuid = Guid.Parse(userUuid);
                forgotPasswordRecoveryTokenRequestDto.UserTypeId = byte.Parse(userTypeId);

                await LogDebugAsync("CheckRecoveryToken Completed", forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Valid("Recovery Token"), 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckRecoveryToken Exception", ex, forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Checking Recovery Token"), 500);
            }
        }

        public async Task<IBuisnessLogicResult> ResetUserPasswordAsync(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto)
        {
            try
            {
                await LogDebugAsync("ResetUserPasswordAsync Started", forgotPasswordRecoveryTokenRequestDto);

                (byte[] newHash, byte[] newSalt) = _passwordUtility.HashPassword(forgotPasswordRecoveryTokenRequestDto.NewPassword);

                switch (forgotPasswordRecoveryTokenRequestDto.UserTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        var admin = await _adminService.GetByUuidAsync(forgotPasswordRecoveryTokenRequestDto.UserUuid);
                        if (admin == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Admin"), 404);
                        }
                        admin.PasswordHash = newHash;
                        admin.PasswordSalt = newSalt;
                        await _adminService.UpdateAdminAsync(admin);
                        break;

                    case (byte)UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(forgotPasswordRecoveryTokenRequestDto.UserUuid);
                        if (staff == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Staff"), 404);
                        }
                        staff.PasswordHash = newHash;
                        staff.PasswordSalt = newSalt;
                        await _staffService.UpdateStaffAsync(staff);
                        break;

                    case (byte)UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(forgotPasswordRecoveryTokenRequestDto.UserUuid);
                        if (student == null)
                        {
                            return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("Student"), 404);
                        }
                        student.PasswordHash = newHash;
                        student.PasswordSalt = newSalt;
                        await _studentService.UpdateStudentAsync(student);
                        break;
                    default:
                        await LogErrorAsync("ResetUserPasswordAsync Invalid User Type", null, forgotPasswordRecoveryTokenRequestDto);
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.Invalid("UserTypeId"), 400);
                        break;
                }

                await LogDebugAsync("ResetUserPasswordAsync Completed", forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicSuccessResult(BuisnessLogicMessage.Successfuly("Password Reseted"), 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("ResetUserPasswordAsync Exception", ex, forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicErrorResult(BuisnessLogicMessage.ErrorOccurred("Reseting Password"), 500);
            }
        }


        #endregion


        #region Security Event Methods
        public async Task<IBuisnessLogicResult> AddSecurityEventRecordAsync(
            HttpContext? httpContext,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            Guid? userGuid = null,
            UserTypeId userTypeId = UserTypeId._,
            string? accessToken = null,
            bool? isEventSuccess = null,
            string? failureMessage = null,
            Dictionary<string, object>? additionalData = null)
        {
            try
            {
                await LogDebugAsync($"{methodName} Started", new
                {
                    AccessToken = accessToken,
                    UserGuid = userGuid,
                    UserTypeId = userTypeId,
                    IsEventSuccess = isEventSuccess,
                    FailureMessage = failureMessage,
                    Description = description,
                    AdditionalData = additionalData
                });

                // User çözümle
                Guid resolvedUserGuid = Guid.Empty;
                UserTypeId resolvedUserTypeId = UserTypeId._;

                if (userGuid.HasValue && userGuid.Value != Guid.Empty && userTypeId != UserTypeId._)
                {
                    resolvedUserGuid = userGuid.Value;
                    resolvedUserTypeId = userTypeId;
                }
                else if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    var sessionUserUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                    var sessionUserTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);

                    if (string.IsNullOrEmpty(sessionUserUuid))
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserUuid", "AccesToken"), 400);
                    }
                    if (string.IsNullOrEmpty(sessionUserTypeId))
                    {
                        return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserTypeId", "AccesToken"), 400);
                    }

                    resolvedUserGuid = Guid.Parse(sessionUserUuid);
                    resolvedUserTypeId = (UserTypeId)byte.Parse(sessionUserTypeId);
                }
                else
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.AtLeastOneRequired(new[] {"(UserUuid & UserTypeId)", "AccessToken"}), 400);
                }

                if (resolvedUserGuid == Guid.Empty)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserUuid"), 400);
                }
                if (resolvedUserTypeId == UserTypeId._)
                {
                    return new BuisnessLogicErrorResult(BuisnessLogicMessage.NotFound("UserTypeId"), 400);
                }

                // Event Type UUID al
                var eventTypeUuid = SecurityEventTypeGuids.EventGuids[eventTypeGuidKey];

                // University UUID al
                var universityUuid = await GetUniversityByUserUuidAsync(resolvedUserTypeId, resolvedUserGuid);

                // SecurityEvent nesnesini oluştur
                SecurityEvent securityEvent = new SecurityEvent
                {
                    EventTypeUuid = eventTypeUuid,
                    UniversityUuid = universityUuid,
                    EventedByAdminUuid = resolvedUserTypeId == UserTypeId.Admin ? resolvedUserGuid : null,
                    EventedByStaffUuid = resolvedUserTypeId == UserTypeId.Staff ? resolvedUserGuid : null,
                    EventedByStudentUuid = resolvedUserTypeId == UserTypeId.Student ? resolvedUserGuid : null,
                    Description = description,
                    IpAddress = httpContext?.Connection?.RemoteIpAddress?.IsIPv4MappedToIPv6 == true
                        ? httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
                        : httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown",
                    Success = isEventSuccess,
                    UserAgent = httpContext?.Request?.Headers["User-Agent"].FirstOrDefault() ?? "Unknown",
                    AdditionalData = additionalData != null ? JsonSerializer.Serialize(additionalData) : null
                };

                bool isAdded = await _securityEventService.RecordSecurityEventAsync(securityEvent);

                if (!isAdded)
                {
                    return new BuisnessLogicErrorResult($"Failed to add {methodName} security event record", 500);
                }

                await LogDebugAsync($"{methodName} Completed", new
                {
                    ResolvedUserGuid = resolvedUserGuid,
                    ResolvedUserTypeId = resolvedUserTypeId,
                    IsEventSuccess = isEventSuccess,
                    FailureMessage = failureMessage
                });

                return new BuisnessLogicSuccessResult($"{methodName} security event record added successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync($"{methodName} Excepted", ex, new { userGuid, userTypeId, accessToken, isEventSuccess, failureMessage, description, additionalData });
                return new BuisnessLogicErrorResult($"{methodName} işlemi sırasında hata oluştu", 500);
            }
        }
        #endregion


        #region Private Methods
        private async Task<Guid?> GetUniversityByUserUuidAsync(UserTypeId userTypeId, Guid userGuid)
        {
            return userTypeId switch
            {
                UserTypeId.Admin => (await _adminService.GetByUuidAsync(userGuid))?.UniversityUuid,
                UserTypeId.Staff => (await _staffService.GetByUuidAsync(userGuid))?.UniversityUuid,
                UserTypeId.Student => (await _studentService.GetByUuidAsync(userGuid))?.UniversityUuid,
                _ => null
            };
        }
        #endregion
    }
}
