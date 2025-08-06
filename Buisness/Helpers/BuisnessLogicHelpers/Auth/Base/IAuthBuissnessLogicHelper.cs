using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.DTOs.AuthDtos.SignInDtos;
using Buisness.DTOs.AuthDtos.SignUpDtos;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Helpers.Common.HelperEnums;
using Buisness.Services.EntityRepositoryServices.Base;
using Core.Enums;
using Core.Utilities.BuisnessLogic.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Buisness.Helpers.BuisnessLogicHelpers.Auth.Base
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

        #region Verify

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

        #region Verify Email
        Task<IBuisnessLogicResult> CheckIsEmailNotVerifiedAsync(string accessToken);
        Task<IBuisnessLogicResult> SendEmailVerificationOTPAsync(string accessToken);
        Task<IBuisnessLogicResult> CheckAndVerifyEmailOTPAsync(string accessToken, string otpCode);
        Task<IBuisnessLogicResult> CompleteEmailVerificaitonAsync(string accessToken);
        #endregion
        
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
}
