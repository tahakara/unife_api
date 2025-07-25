using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Helpers.HelperEnums;
using Buisness.Services.EntityRepositoryServices.Base;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Services.UtilityServices.Base.EmailServices;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Core.Enums;
using Core.Utilities.BuisnessLogic.Base;
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
    public interface IAuthBuisnessLogicHelper : IBuisnessLogicHelper, IServiceManagerBase
    {
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken);
        Task<IBuisnessLogicResult> BlacklistSessionsAsync(string accessToken, BlacklistMode mode);
        Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto);
        Task<IBuisnessLogicResult> RefreshAccessTokenAsync(RefreshTokenResponseDto refreshTokenResponseDto);
        Task<IBuisnessLogicResult> CheckAndCreateSignUpCredentialsAsync(SignUpRequestDto signUpRequestDto, SignUpResponseDto signUpResponseDto);
        Task<IBuisnessLogicResult> PreventSignInBruteForceAsync(SignInRequestDto signInRequestDto);
        Task<IBuisnessLogicResult> CheckUserSessionCountExceededAsync(SignInResponseDto signInResponseDto);
        Task<IBuisnessLogicResult> CheckSignInCredentialsAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto);
        Task<IBuisnessLogicResult> SendSignInOTPAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto);
        Task<IBuisnessLogicResult> SignInCompletedAsync(VerifyOTPRequestDto verifyOTPRequestDto, HttpContext httpContext);
        Task<IBuisnessLogicResult> CheckVerifyOTPAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto);
        Task<IBuisnessLogicResult> CreateSessionAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto);
        Task<IBuisnessLogicResult> RevokeOldOTPAsync(SignInRequestDto signInRequestDto);

        /// <summary>
        /// Adds a security event record to the database.
        /// </summary>
        /// <typeparam name="TKey">The type of the additional data key.</typeparam>
        /// <typeparam name="TValue">The type of the additional data value.</typeparam>
        /// <param name="httpContext">The HTTP context for retrieving request details.</param>
        /// <param name="eventTypeUuid">The unique identifier of the event type.</param>
        /// <param name="securityEventTypeGuid">The GUID of the security event type.</param>
        /// <param name="userTypeId">The ID of the user type related to the event.</param>
        /// <param name="userUuid">The unique identifier of the user related to the event.</param>
        /// <param name="description">
        /// The event description text.  
        /// Only the first 1000 characters will be stored;  
        /// if a longer string is provided, it will be automatically truncated.
        /// </param>
        /// <param name="additionalData">Additional data related to the security event.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<IBuisnessLogicResult> AddSecurityEventRecordAsync(
            HttpContext? httpContext,
            Guid securityEventTypeGuid,
            UserTypeId userTypeId,
            Guid userUuid,
            Guid? universityUuid,
            bool? isEventSuccess,
            string description,
            Dictionary<string, object>? additionalData);
        Task<IBuisnessLogicResult> AddGenericSecurityEventRecordAsync(
            HttpContext? httpContext,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            Guid? userGuid = null,
            UserTypeId userTypeId = UserTypeId._,
            string? accessToken = null,
            bool? isEventSuccess = null,
            string? failureMessage = null);
        Task<IBuisnessLogicResult> AddSecurityEventRecordByTypeAsync(
            HttpContext? httpContext,
            string accessToken,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            bool? isEventSuccess = null,
            string? failureMessage = null);

        Task<IBuisnessLogicResult> AddSignInOTPResendEventRecordAsync (HttpContext? httpContext,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            Guid? userGuid = null,
            byte userTypeId = 0,
            string? accessToken = null,
            bool? isEventSuccess = null,
            string? failureMessage = null);

        Task<IBuisnessLogicResult> ValidatePasswordAsync(string accessToken, string password);

        Task<IBuisnessLogicResult> ChangePasswordAsync(string accessToken, string oldPassword, string newPassword);
        Task<IBuisnessLogicResult> CheckForgotPasswordCredentialsAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<IBuisnessLogicResult> PreventForgotBruteForceAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<IBuisnessLogicResult> SendRecoveryNotificationAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<IBuisnessLogicResult> CheckRecoveryToken(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto);
        Task<IBuisnessLogicResult> ResetUserPasswordAsync(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto);

        // TODO: Uncomment when implemented
        //Task<IBuisnessLogicResult> RevokeSignInBruteForceTokenAsync(VerifyOTPRequestDto verifyOTPRequestDto);
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
                await LogDebugAsync("IsAccessTokenValidAsync Started", accessToken);

                bool isValid = await _sessionJwtService.ValidateTokenAsync(accessToken);
                if (!isValid)
                {
                    return new BuisnessLogicErrorResult("AccessToken invalid or expired");
                }

                await LogDebugAsync("IsAccessTokenValidAsync Completed", accessToken);
                return new BuisnessLogicSuccessResult("Access token validation successful", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("IsAccessTokenValidAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult("Access token validation failed", 500);
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
                _ => new BuisnessLogicErrorResult("Invalid blacklist mode", 400)
            };
        }

        private async Task<IBuisnessLogicResult> BlacklistSingleSessionTokenAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("BlacklistSingleSessionTokenAsync Started", accessToken);

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var sessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(accessToken);
                var userTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid) || string.IsNullOrEmpty(userTypeId))
                {
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are not found on AccessToken", 400);
                }

                var revokeResult = await _sessionJwtService.RevokeTokensAsync(userUuid, sessionUuid, userTypeId);
                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke tokens", 500);
                }

                await LogDebugAsync("BlacklistSingleSessionTokenAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid, SessionUuid = sessionUuid });
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistSingleSessionTokenAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
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
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are not found on AccessToken", 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensExcluededByOne(userUuid, currentSessionUuid);

                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke tokens", 500);
                }

                await LogDebugAsync("BlacklistAllSessionsExceptOneAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid, CurrentSessionUuid = currentSessionUuid });
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistAllSessionsExceptOneAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
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
                    return new BuisnessLogicErrorResult("User UUID is not found on AccessToken", 400);
                }

                bool revokeResult = await _sessionJwtService.RevokeUserAllTokensAsync(userUuid);

                if (!revokeResult)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke all session tokens", 500);
                }

                await LogDebugAsync("BlacklistAllSessionTokensByUserAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid });
                return new BuisnessLogicSuccessResult("All session tokens blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistAllSessionTokensByUserAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting all session tokens by user", 500);
            }
        }

        #endregion

        public async Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto)
        {
            try
            {
                await LogDebugAsync("IsRefreshTokenValidAsync Started", refreshTokenRequestDto.RefreshToken);

                await ValidateAsync(refreshTokenRequestDto);

                if (string.IsNullOrEmpty(refreshTokenRequestDto.RefreshToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.RefreshToken))
                {
                    return new BuisnessLogicErrorResult("Refresh token is required", 400);
                }

                if (string.IsNullOrEmpty(refreshTokenRequestDto.AccessToken) || string.IsNullOrWhiteSpace(refreshTokenRequestDto.AccessToken))
                {
                    var postfixSearchKey = await _sessionJwtService.GetRefreshTokenKeyByRefreshTokenPostfixAsync(refreshTokenRequestDto.RefreshToken);

                    if (string.IsNullOrEmpty(postfixSearchKey))
                    {
                        return new BuisnessLogicErrorResult("Refresh token invalid or expired", 400);
                    }

                    JsonElement? postfixSearchResult = await _sessionJwtService.GetRefreshTokenValue(postfixSearchKey);
                    if (postfixSearchKey == null)
                    {
                        return new BuisnessLogicErrorResult("Refresh token invalid or expired", 400);
                    }

                    if (!postfixSearchResult.HasValue || !postfixSearchResult.Value.TryGetProperty("UserUuid", out var userUuid) || !postfixSearchResult.Value.TryGetProperty("SessionUuid", out var sessionUuid) || !postfixSearchResult.Value.TryGetProperty("UserTypeId", out var userTypeId))
                    {
                        return new BuisnessLogicErrorResult("Refresh token is not valid or expired", 400);
                    }

                    refreshTokenResponseDto.UserUuid = userUuid.GetString() ?? string.Empty;
                    refreshTokenResponseDto.SessionUuid = sessionUuid.GetString() ?? string.Empty;
                    refreshTokenResponseDto.RefreshToken = refreshTokenRequestDto.RefreshToken;
                    refreshTokenResponseDto.UserTypeId = byte.TryParse(userTypeId.GetString(), out var parsedUserTypeId) ? parsedUserTypeId : (byte)0;

                    return new BuisnessLogicSuccessResult("Refresh token is valid", 200);

                }

                refreshTokenResponseDto.UserUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(refreshTokenRequestDto.AccessToken);
                refreshTokenResponseDto.SessionUuid = await _sessionJwtService.GetSessionUuidFromTokenAsync(refreshTokenRequestDto.AccessToken);
                refreshTokenResponseDto.RefreshToken = refreshTokenRequestDto.RefreshToken;
                if (string.IsNullOrEmpty(refreshTokenResponseDto.UserUuid) || string.IsNullOrEmpty(refreshTokenResponseDto.SessionUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID and Session UUID are not found on AccessToken", 400);
                }

                await LogDebugAsync("IsRefreshTokenValidAsync Completed", new
                {
                    AccessToken = refreshTokenRequestDto.AccessToken,
                    UserUuid = refreshTokenResponseDto.UserUuid,
                    SessionUuid = refreshTokenResponseDto.SessionUuid,
                    RefreshToken = refreshTokenResponseDto.RefreshToken
                });
                return new BuisnessLogicSuccessResult("Refresh token is valid", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("IsRefreshTokenValidAsync Excepted", ex, refreshTokenRequestDto.RefreshToken);
                return new BuisnessLogicErrorResult("IsRefreshTokenValid işlemi sırasında hata oluştu", 500);
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
                    return new BuisnessLogicErrorResult("Refresh token işlemi başarısız oldu", 500);
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
                return new BuisnessLogicSuccessResult("Refresh token Completed", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("RefreshAccessTokenAsync Excepted", ex, refreshTokenResponseDto.RefreshToken);
                return new BuisnessLogicErrorResult("Refresh token işlemi sırasında hata oluştu", 500);
            }
        }

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
                            return new BuisnessLogicErrorResult("Email not usable", 400);
                        }

                        bool phoneNumberResult = await _adminService.IsPhoneNumberExistsAsync(signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);

                        if (phoneNumberResult)
                        {
                            return new BuisnessLogicErrorResult("Phone number not usable", 400);
                        }

                        (byte[] hash, byte[] salt) = _passwordUtility.HashPassword(signUpRequestDto.Password);
                        Admin newAdmin = new Admin();

                        _mapper.Map(signUpRequestDto, newAdmin);

                        newAdmin.PasswordHash = hash;
                        newAdmin.PasswordSalt = salt;

                        newAdmin = await _adminService.CreateNewAdminAsync(newAdmin);

                        if (newAdmin == null)
                        {
                            return new BuisnessLogicErrorResult("Admin creation failed", 500);
                        }

                        _mapper.Map(newAdmin, signUpResponseDto);

                        return new BuisnessLogicSuccessResult("New admin created successfully", 200);
                        break;

                    case (byte)UserTypeId.Staff:
                        bool staffEmailResult = await _staffService.IsEmailExistsAsync(signUpRequestDto.Email);
                        if (staffEmailResult)
                        {
                            return new BuisnessLogicErrorResult("Email not usable", 400);
                        }
                        bool staffPhoneNumberResult = await _staffService.IsPhoneNumberExistsAsync(signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);
                        if (staffPhoneNumberResult)
                        {
                            return new BuisnessLogicErrorResult("Phone number not usable", 400);
                        }

                        (byte[] staffHash, byte[] staffSalt) = _passwordUtility.HashPassword(signUpRequestDto.Password);
                        Staff newStaff = new Staff();

                        _mapper.Map(signUpRequestDto, newStaff);
                        newStaff.PasswordHash = staffHash;
                        newStaff.PasswordSalt = staffSalt;

                        newStaff = await _staffService.CreateNewStaffAsync(newStaff);
                        if (newStaff == null)
                        {
                            return new BuisnessLogicErrorResult("Staff creation failed", 500);
                        }

                        _mapper.Map(newStaff, signUpResponseDto);

                        return new BuisnessLogicSuccessResult("New staff created successfully", 200);
                        break;
                    case (byte)UserTypeId.Student:

                        bool studentEmailResult = await _studentService.IsEmailExistsAsync(signUpRequestDto.Email);
                        if (studentEmailResult)
                        {
                            return new BuisnessLogicErrorResult("Email not usable", 400);
                        }

                        bool studentPhoneNumberResult = await _studentService.IsPhoneNumberExistsAsync(signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);

                        if (studentPhoneNumberResult)
                        {
                            return new BuisnessLogicErrorResult("Phone number not usable", 400);
                        }

                        (byte[] studentHash, byte[] studentSalt) = _passwordUtility.HashPassword(signUpRequestDto.Password);
                        Student newStudent = new Student();

                        _mapper.Map(signUpRequestDto, newStudent);
                        newStudent.PasswordHash = studentHash;
                        newStudent.PasswordSalt = studentSalt;

                        newStudent = await _studentService.CreateNewStudentAsync(newStudent);

                        if (newStudent == null)
                        {
                            return new BuisnessLogicErrorResult("Student creation failed", 500);
                        }

                        _mapper.Map(newStudent, signUpResponseDto);

                        return new BuisnessLogicSuccessResult("New student created successfully", 200);
                        break;

                    default:
                        return new BuisnessLogicErrorResult("Geçersiz kullanıcı tipi", 400);
                        break;
                }
                return new BuisnessLogicErrorResult("Kullanıcı tipi bulunamadı", 400);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckSignUpCredentialsAsync Excepted", ex, signUpRequestDto);
                return new BuisnessLogicErrorResult("CheckSignUpCredentials işlemi sırasında hata oluştu", 500);
            }
        }

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
                        else { return new BuisnessLogicErrorResult("Missing credentials: Email or PhoneCountryCode and PhoneNumber are required", 400); }

                        if (currentAdmin == null)
                        {
                            return new BuisnessLogicErrorResult("Admin not found", 404);
                        }

                        if (!_passwordUtility.VerifyPassword(signInRequestDto.Password, currentAdmin.PasswordHash, currentAdmin.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid password", 401);
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
                        else { return new BuisnessLogicErrorResult("Missing credentials: Email or PhoneCountryCode and PhoneNumber are required", 400); }

                        if (currentStaff == null)
                        {
                            return new BuisnessLogicErrorResult("Staff not found", 404);
                        }

                        if (!_passwordUtility.VerifyPassword(signInRequestDto.Password, currentStaff.PasswordHash, currentStaff.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid password", 401);
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
                        else { return new BuisnessLogicErrorResult("Missing credentials: Email or PhoneCountryCode and PhoneNumber are required", 400); }

                        if (currentStudent == null)
                        {
                            return new BuisnessLogicErrorResult("Student not found", 404);
                        }

                        if (!_passwordUtility.VerifyPassword(signInRequestDto.Password, currentStudent.PasswordHash, currentStudent.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid password", 401);
                        }

                        signInRequestDto.UserUuid = currentStudent.StudentUuid;

                        signInResponseDto.UserUuid = currentStudent.StudentUuid;
                        signInResponseDto.UserTypeId = (byte)UserTypeId.Student;
                        break;

                    default:
                        return new BuisnessLogicErrorResult("Invalid user type", 400);
                        break;
                }

                await LogDebugAsync("CheckSignInCredentialsAsync Completed", signInRequestDto);
                return new BuisnessLogicSuccessResult("SignIn credentials are valid", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckSignInCredentialsAsync Excepted", ex, signInRequestDto);
                return new BuisnessLogicErrorResult("CheckSignInCredentials işlemi sırasında hata oluştu", 500);
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
                            return new BuisnessLogicErrorResult("Failed to set OTP code", 500);
                        }

                        bool mailResult = await _emailService.SendSignInOtpCode(signInRequestDto.Email, signInRequestDto.OtpCode);
                        if (!mailResult)
                        {
                            return new BuisnessLogicErrorResult("Failed to send OTP via Email", 500);
                        }
                        break;

                    //case (byte)OTPTypeId.Sms:
                    //    bool smsResult = await _emailService.SendSignInOtpCode(signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber, signInRequestDto.OtpCode);
                    //    break;

                    default:
                        return new BuisnessLogicErrorResult("Invalid OTP type", 400);
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
                return new BuisnessLogicSuccessResult("SendSignInOTP Completed", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("SendSignInOTPAsync Excepted", ex, signInRequestDto);
                return new BuisnessLogicErrorResult("SendSignInOTP işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> CheckVerifyOTPAsync(
            VerifyOTPRequestDto verifyOTPRequestDto,
            VerifyOTPResponseDto verifyOTPResponseDto)
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
                    return new BuisnessLogicErrorResult("OTP code is invalid or expired", 400);
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

                    return new BuisnessLogicErrorResult("Too many OTP attempts. OTP deleted.", 400);
                }

                // Is OTP code provided?
                if (string.IsNullOrWhiteSpace(verifyOTPRequestDto.OtpCode))
                    return new BuisnessLogicErrorResult("OTP code must be provided", 400);

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
                        return new BuisnessLogicErrorResult("Failed to update OTP attempt", 500);

                    return new BuisnessLogicErrorResult("Invalid OTP code", 400);
                }

                // Is OTP code valid?
                bool removed = await _OTPCodeService.RemoveCodeAsync(
                    verifyOTPRequestDto.SessionUuid.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.OtpTypeId.ToString(),
                    verifyOTPRequestDto.OtpCode);

                if (!removed)
                    return new BuisnessLogicErrorResult("Failed to delete OTP code", 500);

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

                return new BuisnessLogicSuccessResult("OTP verified successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckVerifyOTPAsync Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult("CheckVerifyOTP işleminde hata oluştu", 500);
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
                return new BuisnessLogicSuccessResult("Session created successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CreateSessionAsync Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult("CreateSessionAsync işlemi sırasında hata oluştu", 500);
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
                    return new BuisnessLogicErrorResult("Failed to revoke old OTP", 500);
                }

                await LogDebugAsync("RevokeOldOTP Completed", new
                {
                    UserType = signInRequestDto.UserTypeId,
                    SessionUuid = signInRequestDto.SessionUuid,
                    OtpTypeId = signInRequestDto.OtpTypeId
                });
                return new BuisnessLogicSuccessResult("RevokeOldOTP Completed", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("RevokeOldOTPAsync Excepted", ex, signInRequestDto);
                return new BuisnessLogicErrorResult("RevokeOldOTP işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> AddSecurityEventRecordAsync(
            HttpContext? httpContext,
            Guid securityEventTypeGuid,
            UserTypeId userTypeId,
            Guid userUuid,
            Guid? universityUuid,
            bool? isEventSuccess,
            string description,
            Dictionary<string, object>? additionalData)
        {
            try
            {
                await LogDebugAsync("AddSecurityEventRecordAsync Started", new
                {
                    SecurityEventTypeGuid = securityEventTypeGuid,
                    UserTypeId = userTypeId,
                    UserUuid = userUuid,
                    Description = description,
                    AdditionalData = additionalData
                });

                SecurityEvent securityEvent = new SecurityEvent
                {
                    EventTypeUuid = securityEventTypeGuid,
                    UniversityUuid = universityUuid,
                    EventedByAdminUuid = userTypeId == UserTypeId.Admin ? userUuid : null,
                    EventedByStaffUuid = userTypeId == UserTypeId.Staff ? userUuid : null,
                    EventedByStudentUuid = userTypeId == UserTypeId.Student ? userUuid : null,
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
                    return new BuisnessLogicErrorResult("Failed to add security event record", 500);
                }
                return new BuisnessLogicSuccessResult("Security event record added successfully", 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("AddSecurityEventRecordAsync Excepted", ex, new { securityEventTypeGuid, userTypeId, userUuid, description, additionalData });
                return new BuisnessLogicErrorResult("AddSecurityEventRecord işlemi sırasında hata oluştu", 500);
            }
        }


        public async Task<IBuisnessLogicResult> AddGenericSecurityEventRecordAsync(
            HttpContext? httpContext,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            Guid? userGuid = null,
            UserTypeId userTypeId = UserTypeId._,
            string? accessToken = null,
            bool? isEventSuccess = null,
            string? failureMessage = null)
        {
            try
            {
                await LogDebugAsync($"{methodName} Started", new
                {
                    AccessToken = accessToken,
                    UserGuid = userGuid,
                    UserTypeId = userTypeId,
                    IsEventSuccess = isEventSuccess,
                    FailureMessage = failureMessage
                });

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
                        return new BuisnessLogicErrorResult("User UUID is not found on AccessToken", 400);
                    }
                    if (string.IsNullOrEmpty(sessionUserTypeId))
                    {
                        return new BuisnessLogicErrorResult("User Type ID is not found on AccessToken", 400);
                    }

                    resolvedUserGuid = Guid.Parse(sessionUserUuid);
                    resolvedUserTypeId = (UserTypeId)byte.Parse(sessionUserTypeId);
                }
                else
                {
                    return new BuisnessLogicErrorResult("Either userGuid/userTypeId or accessToken must be provided", 400);
                }

                if (resolvedUserGuid == Guid.Empty)
                {
                    return new BuisnessLogicErrorResult("User UUID could not be resolved", 400);
                }
                if (resolvedUserTypeId == UserTypeId._)
                {
                    return new BuisnessLogicErrorResult("User Type ID could not be resolved", 400);
                }

                var eventTypeUuid = SecurityEventTypeGuids.EventGuids[eventTypeGuidKey];

                var universityUuid = await GetUniversityByUserUuidAsync(resolvedUserTypeId, resolvedUserGuid);

                IBuisnessLogicResult result = await AddSecurityEventRecordAsync(
                    httpContext,
                    eventTypeUuid,
                    resolvedUserTypeId,
                    resolvedUserGuid,
                    universityUuid,
                    isEventSuccess,
                    description,
                    null);

                if (!result.Success)
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
                await LogErrorAsync($"{methodName} Excepted", ex, new { userGuid, userTypeId, accessToken, isEventSuccess, failureMessage });
                return new BuisnessLogicErrorResult($"{methodName} işlemi sırasında hata oluştu", 500);
            }
        }


        public async Task<IBuisnessLogicResult> AddSecurityEventRecordByTypeAsync(
            HttpContext? httpContext,
            string accessToken,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            bool? isEventSuccess = null,
            string? failureMessage = null)
        {
            return await AddGenericSecurityEventRecordAsync(
                httpContext,
                eventTypeGuidKey,
                methodName,
                description,
                null,
                UserTypeId._,
                accessToken,
                isEventSuccess,
                failureMessage);
        }


        public async Task<IBuisnessLogicResult> AddSignInOTPResendEventRecordAsync(HttpContext? httpContext,
            SecurityEventTypeGuid eventTypeGuidKey,
            string methodName,
            string description,
            Guid? userGuid = null,
            byte userTypeId = 0,
            string? accessToken = null,
            bool? isEventSuccess = null,
            string? failureMessage = null)
        {
            return await AddGenericSecurityEventRecordAsync(
                httpContext,
                eventTypeGuidKey,
                methodName,
                description,
                userGuid,
                (UserTypeId)userTypeId,
                accessToken,
                isEventSuccess,
                failureMessage);
        }



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

        public async Task<IBuisnessLogicResult> ValidatePasswordAsync(string accessToken, string password)
        {
            try
            {
                await LogDebugAsync("ValidatePasswordAsync Started", new { AccessToken = accessToken, Password = password });

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(accessToken);
                var userTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);
                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(userTypeId))
                {
                    return new BuisnessLogicErrorResult("User UUID or User Type ID could not be resolved from access token", 400);
                }
                Guid uuid = Guid.Parse(userUuid);
                UserTypeId typeId = (UserTypeId)byte.Parse(userTypeId);
                switch (typeId)
                {
                    case UserTypeId.Admin:
                        var admin = await _adminService.GetByUuidAsync(uuid);
                        if (admin == null || !_passwordUtility.VerifyPassword(password, admin.PasswordHash, admin.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid password for Admin", 401);
                        }
                        break;
                    case UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(uuid);
                        if (staff == null || !_passwordUtility.VerifyPassword(password, staff.PasswordHash, staff.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid password for Staff", 401);
                        }
                        break;
                    case UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(uuid);
                        if (student == null || !_passwordUtility.VerifyPassword(password, student.PasswordHash, student.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid password for Student", 401);
                        }
                        break;
                    default:
                        return new BuisnessLogicErrorResult("Invalid user type", 400);
                }
                await LogDebugAsync("ValidatePasswordAsync Completed", new { UserUuid = uuid, UserTypeId = typeId });
                return new BuisnessLogicSuccessResult("Password is correct", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("ValidatePasswordAsync Excepted", ex, new { AccessToken = accessToken, Password = password });
                return new BuisnessLogicErrorResult("ValidatePasswordAsync işlemi sırasında hata oluştu", 500);
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
                    return new BuisnessLogicErrorResult("User UUID could not be resolved from access token", 400);
                }
                var userTypeId = await _sessionJwtService.GetUserTypeIdFromTokenAsync(accessToken);
                if (string.IsNullOrEmpty(userTypeId))
                {
                    return new BuisnessLogicErrorResult("User Type ID could not be resolved from access token", 400);
                }

                Guid uuid = Guid.Parse(userUuid);
                UserTypeId typeId = (UserTypeId)byte.Parse(userTypeId);
                switch (typeId)
                {
                    case UserTypeId.Admin:
                        var admin = await _adminService.GetByUuidAsync(uuid);
                        if (admin == null || !_passwordUtility.VerifyPassword(oldPassword, admin.PasswordHash, admin.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid old password for Admin", 401);
                        }
                        (admin.PasswordSalt, admin.PasswordHash) = _passwordUtility.HashPassword(newPassword);
                        await _adminService.UpdateAdminAsync(admin);
                        break;
                    case UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(uuid);
                        if (staff == null || !_passwordUtility.VerifyPassword(oldPassword, staff.PasswordHash, staff.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid old password for Staff", 401);
                        }
                        (staff.PasswordSalt, staff.PasswordHash) = _passwordUtility.HashPassword(newPassword);
                        await _staffService.UpdateStaffAsync(staff);
                        break;
                    case UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(uuid);
                        if (student == null || !_passwordUtility.VerifyPassword(oldPassword, student.PasswordHash, student.PasswordSalt))
                        {
                            return new BuisnessLogicErrorResult("Invalid old password for Student", 401);
                        }
                        (student.PasswordSalt, student.PasswordHash) = _passwordUtility.HashPassword(newPassword);
                        await _studentService.UpdateStudentAsync(student);
                        break;
                    default:
                        return new BuisnessLogicErrorResult("Invalid user type", 400);
                }

                await LogDebugAsync("ChangePasswordAsync Completed", new { UserUuid = uuid, UserTypeId = typeId });
                return new BuisnessLogicSuccessResult("Password changed successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("ChangePasswordAsync Excepted", ex, new { AccessToken = accessToken, OldPassword = oldPassword, NewPassword = newPassword });
                return new BuisnessLogicErrorResult("ChangePassword işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> PreventSignInBruteForceAsync(SignInRequestDto signInRequestDto)
        {
            try
            {
                await LogDebugAsync("PreventSignInBruteForceAsync başladı");

                var key = _sessionJwtService.GenerateSignInOTPBruteForceProtectionKey(signInRequestDto.Email, signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);

                if (string.IsNullOrEmpty(key))
                {
                    return new BuisnessLogicErrorResult("Geçersiz brute force key", 400);
                }

                bool exists = await _sessionJwtService.IsSignInOTPBruteForceProtectionKeyExistsAsync(key);

                if (exists)
                {
                    int attempts = 0;
                    attempts = await _sessionJwtService.GetSignInOTPBruteForceProtectionAttemptsByKeyAsync(key);
                    if (attempts >= 5)
                    {
                        return new BuisnessLogicErrorResult("Çok fazla başarısız giriş denemesi, lütfen daha sonra tekrar deneyin.", 429);
                    }
                    await _sessionJwtService.IncrementSignInOTPBruteForceProtectionAttemptsAsync(key);

                    return new BuisnessLogicSuccessResult("Brute force koruması geçti", 200);
                }

                // Increment or set
                await _sessionJwtService.SetSignInOTPBruteForceProtectionKeyAsync(key);

                return new BuisnessLogicSuccessResult("Brute force koruması geçti", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PreventSignInBruteForceAsync hata");
                return new BuisnessLogicErrorResult("Brute force koruması sırasında hata oluştu", 500);
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
                    return new BuisnessLogicErrorResult("User session count exceeded", 403);
                }

                await LogDebugAsync("CheckUserSessionCountExceeded Completed", signInResponseDto);
                return new BuisnessLogicSuccessResult("CheckUserSessionCountExceeded Completed", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckUserSessionCountExceeded Excepted", ex, signInResponseDto);
                return new BuisnessLogicErrorResult("CheckUserSessionCountExceeded işlemi sırasında hata oluştu", 500);
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
                    return new BuisnessLogicErrorResult("SignInResponseDto is null or UserUuid is empty", 400);
                }

                switch (verifyOTPRequestDto.UserTypeId)
                {
                    case (byte)UserTypeId.Admin:
                        Admin admin = await _adminService.GetByUuidAsync(verifyOTPRequestDto.UserUuid);
                        if (admin == null)
                        {
                            return new BuisnessLogicErrorResult("Admin not found", 404);
                        }
                        await _emailService.SendSiginCompleteMail(ipAddress, userAgent, admin.Email, admin.FirstName, admin.MiddleName, admin.LastName);
                        break;

                    case (byte)UserTypeId.Staff:
                        Staff staff = await _staffService.GetByUuidAsync(verifyOTPRequestDto.UserUuid);
                        if (staff == null)
                        {
                            return new BuisnessLogicErrorResult("Staff not found", 404);
                        }
                        await _emailService.SendSiginCompleteMail(ipAddress, userAgent, staff.Email, staff.FirstName, staff.MiddleName, staff.LastName);
                        break;
                    case (byte)UserTypeId.Student:
                        Student student = await _studentService.GetByUuidAsync(verifyOTPRequestDto.UserUuid);
                        if (student == null)
                        {
                            return new BuisnessLogicErrorResult("Student not found", 404);
                        }
                        await _emailService.SendSiginCompleteMail(ipAddress, userAgent, student.Email, student.FirstName, student.MiddleName, student.LastName);
                        break;
                    default:
                        return new BuisnessLogicErrorResult("Invalid user type", 400);
                        break;
                }
                return new BuisnessLogicSuccessResult("SiginCompleted işlemi başarılı", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("SiginCompleted Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult("SiginCompleted işlemi sırasında hata oluştu", 500);
            }
        }

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
                    return new BuisnessLogicErrorResult("Email veya telefon bilgileri sağlanmalıdır.", 400);
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
                                    return new BuisnessLogicErrorResult("Admin için geçersiz kurtarma yöntemi.", 400);
                            }

                            if (admin == null)
                                return new BuisnessLogicErrorResult("Girilen bilgilerle eşleşen admin bulunamadı.", 404);

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
                                    return new BuisnessLogicErrorResult("Staff için geçersiz kurtarma yöntemi.", 400);
                            }

                            if (staff == null)
                                return new BuisnessLogicErrorResult("Girilen bilgilerle eşleşen staff bulunamadı.", 404);

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
                                    return new BuisnessLogicErrorResult("Student için geçersiz kurtarma yöntemi.", 400);
                            }

                            if (student == null)
                                return new BuisnessLogicErrorResult("Girilen bilgilerle eşleşen student bulunamadı.", 404);

                            forgotPasswordRequestDto.UserUuid = student.StudentUuid;
                        }
                        break;

                    default:
                        return new BuisnessLogicErrorResult("Geçersiz kullanıcı tipi.", 400);
                }

                await LogDebugAsync("CheckForgotPasswordCredentialsAsync Completed", new
                {
                    forgotPasswordRequestDto.UserTypeId,
                    forgotPasswordRequestDto.Email,
                    forgotPasswordRequestDto.PhoneCountryCode,
                    forgotPasswordRequestDto.PhoneNumber
                });

                return new BuisnessLogicSuccessResult("Şifre sıfırlama kimlik doğrulama kontrolü başarılı.", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckForgotPasswordCredentialsAsync Exception", ex, forgotPasswordRequestDto);
                return new BuisnessLogicErrorResult("Şifre sıfırlama kimlik doğrulama sırasında bir hata oluştu.", 500);
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
                        return new BuisnessLogicErrorResult("Şifre sıfırlama işlemi için zaten bir oturum var.", 400);
                    }

                    forgotPasswordRequestDto.RecoveryToken = await _sessionJwtService.SetForgotBruteForceProtectionKeyAsync(
                        Guid.NewGuid().ToString(),
                        forgotPasswordRequestDto.UserTypeId.ToString(),
                        forgotPasswordRequestDto.UserUuid.ToString(),
                        forgotPasswordRequestDto.Email,
                        forgotPasswordRequestDto.PhoneCountryCode,
                        forgotPasswordRequestDto.PhoneNumber);

                    await LogDebugAsync("PreventForgotBruteForceAsync Completed", forgotPasswordRequestDto);
                    return new BuisnessLogicSuccessResult("Şifre sıfırlama oturumu oluşturuldu.", 200);
                }

                // takes key and session uuid
                key = await _sessionJwtService.GetForgotBruteForceProtectionKeyByRecoverySessionUuidAsync(
                    forgotPasswordRequestDto.RecoverySessionUuid.ToString());
                if (!string.IsNullOrEmpty(key))
                {
                    return new BuisnessLogicErrorResult("Şifre sıfırlama işlemi için zaten bir oturum var.", 400);
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
                    return new BuisnessLogicErrorResult("Şifre sıfırlama oturumu oluşturulamadı.", 500);
                }

                await LogDebugAsync("PreventForgotBruteForceAsync Completed", forgotPasswordRequestDto);
                return new BuisnessLogicSuccessResult("PreventForgotBruteForce işlemi başarılı", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("PreventForgotBruteForceAsync Exception", ex, forgotPasswordRequestDto);
                return new BuisnessLogicErrorResult("PreventForgotBruteForce işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> SendRecoveryNotificationAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            try
            {
                await LogDebugAsync("SendRecoveryNotificationAsync Started", forgotPasswordRequestDto);

                if (string.IsNullOrEmpty(forgotPasswordRequestDto.RecoveryToken))
                {
                    return new BuisnessLogicErrorResult("Şifre sıfırlama oturumu bulunamadı.", 400);
                }

                switch (forgotPasswordRequestDto.RecoveryMethodId)
                {
                    case 1:
                        // Email ile bildirim gönder
                        if (string.IsNullOrEmpty(forgotPasswordRequestDto.Email))
                        {
                            return new BuisnessLogicErrorResult("Email bilgisi sağlanmalıdır.", 400);
                        }
                        bool result = await _emailService.SendForgotPasswordEmailAsync(
                            forgotPasswordRequestDto.Email,
                            forgotPasswordRequestDto.RecoveryToken);
                        if (!result)
                            return new BuisnessLogicErrorResult("Email gönderilemedi.", 500);
                        return new BuisnessLogicSuccessResult("Email gönderildi.", 200);
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
                        return new BuisnessLogicErrorResult("Geçersiz kurtarma yöntemi.", 400);
                        break;
                }
                return new BuisnessLogicErrorResult("SendRecoveryNotification işlemi başarılı", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("SendRecoveryNotificationAsync Exception", ex, forgotPasswordRequestDto);
                return new BuisnessLogicErrorResult("SendRecoveryNotification işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> CheckRecoveryToken(ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto)
        {
            try
            {
                await LogDebugAsync("CheckRecoveryToken Started", forgotPasswordRecoveryTokenRequestDto);
                if (string.IsNullOrEmpty(forgotPasswordRecoveryTokenRequestDto.RecoveryToken))
                {
                    return new BuisnessLogicErrorResult("Recovery token is required", 400);
                }
                var isValid = await _sessionJwtService.IsForgotBruteForceProtectionKeyExistsAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);
                if (!isValid)
                {
                    return new BuisnessLogicErrorResult("Invalid or expired recovery token", 400);
                }
                string sessionUuid = await _sessionJwtService.GetForgotBruteForceProtectionSessionUuidByRecoveryTokenAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);
                string userUuid = await _sessionJwtService.GetForgotBruteForceProtectionUserUuidByRecoveryTokenAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);
                string userTypeId = await _sessionJwtService.GetForgotBruteForceProtectionUserTypeIdByRecoveryTokenAsync(forgotPasswordRecoveryTokenRequestDto.RecoveryToken);


                forgotPasswordRecoveryTokenRequestDto.RecoverySessionUuid = Guid.Parse(sessionUuid);
                forgotPasswordRecoveryTokenRequestDto.UserUuid = Guid.Parse(userUuid);
                forgotPasswordRecoveryTokenRequestDto.UserTypeId = byte.Parse(userTypeId);

                await LogDebugAsync("CheckRecoveryToken Completed", forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicSuccessResult("Recovery token is valid", 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckRecoveryToken Exception", ex, forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicErrorResult("CheckRecoveryToken işlemi sırasında hata oluştu", 500);
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
                            return new BuisnessLogicErrorResult("Admin not found", 404);
                        }
                        admin.PasswordHash = newHash;
                        admin.PasswordSalt = newSalt;
                        await _adminService.UpdateAdminAsync(admin);
                        break;

                    case (byte)UserTypeId.Staff:
                        var staff = await _staffService.GetByUuidAsync(forgotPasswordRecoveryTokenRequestDto.UserUuid);
                        if (staff == null)
                        {
                            return new BuisnessLogicErrorResult("Staff not found", 404);
                        }
                        staff.PasswordHash = newHash;
                        staff.PasswordSalt = newSalt;
                        await _staffService.UpdateStaffAsync(staff);
                        break;

                    case (byte)UserTypeId.Student:
                        var student = await _studentService.GetByUuidAsync(forgotPasswordRecoveryTokenRequestDto.UserUuid);
                        if (student == null)
                        {
                            return new BuisnessLogicErrorResult("Student not found", 404);
                        }
                        student.PasswordHash = newHash;
                        student.PasswordSalt = newSalt;
                        await _studentService.UpdateStudentAsync(student);
                        break;
                    default:
                        await LogErrorAsync("ResetUserPasswordAsync Invalid User Type", null, forgotPasswordRecoveryTokenRequestDto);
                        return new BuisnessLogicErrorResult("Invalid user type", 400);
                        break;
                }

                await LogDebugAsync("ResetUserPasswordAsync Completed", forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicSuccessResult("Password reset successfully", 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("ResetUserPasswordAsync Exception", ex, forgotPasswordRecoveryTokenRequestDto);
                return new BuisnessLogicErrorResult("ResetUserPasswordAsync işlemi sırasında hata oluştu", 500);
            }
        }

        // TODO :  
        //public async Task<IBuisnessLogicResult> RevokeSignInBruteForceTokenAsync(VerifyOTPRequestDto verifyOTPRequestDto)
        //{
        //    throw new NotImplementedException("RevokeSignInBruteForceTokenAsync method is not implemented yet.");
        //}
    }
}
