using AutoMapper;
using Buisness.Abstract.DtoBase.Base;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Abstract.ServicesBase.Base;
using Buisness.Concrete.Dto;
using Buisness.Concrete.ServiceManager;
using Buisness.DTOs.AuthDtos;
using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Services.UtilityServices;
using Buisness.Services.UtilityServices.Base.EmailServices;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Core.Utilities.BuisnessLogic.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.DataResults;
using Core.Utilities.OTPUtilities;
using Core.Utilities.OTPUtilities.Base;
using Core.Utilities.PasswordUtilities;
using Core.Utilities.PasswordUtilities.Base;
using Core.Abstract;
using Core.Enums;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public interface IAuthBuisnessLogicHelper : IBuisnessLogicHelper, IServiceManagerBase
    {
        Task<IBuisnessLogicResult> IsAccessTokenValidAsync(string accessToken);
        Task<IBuisnessLogicResult> BlackListSessionTokensForASingleSessionAsync(string accessToken);
        Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string accessToken);
        Task<IBuisnessLogicResult> BlacklistSessionsExcludedByOneAsync(string accessToken);
        Task<IBuisnessLogicResult> IsRefreshTokenValidAsync(RefreshTokenRequestDto refreshTokenRequestDto, RefreshTokenResponseDto refreshTokenResponseDto);
        Task<IBuisnessLogicResult> RefreshAccessTokenAsync(RefreshTokenResponseDto refreshTokenResponseDto);
        Task<IBuisnessLogicResult> CheckAndCreateSignUpCredentialsAsync(SignUpRequestDto signUpRequestDto, SignUpResponseDto signUpResponseDto);
        Task<IBuisnessLogicResult> CheckSignInCredentialsAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto);
        Task<IBuisnessLogicResult> SendSignInOTPAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto);
        Task<IBuisnessLogicResult> CheckVerifyOTPAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto);
        Task<IBuisnessLogicResult> CreatSession(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto);
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
        Task<IBuisnessLogicResult> AddSecurityEventRecordAsync<TKey, TValue>(HttpContext? httpContext, Guid securityEventTypeGuid, UserTypeId userTypeId, Guid userUuid, Guid? universityUuid, string description, Dictionary<TKey, TValue>? additionalData);
        Task<IBuisnessLogicResult> AddLogoutSecurityEventRecordAsync(HttpContext? httpContext ,LogoutRequestDto logoutRequestDto, bool isEventSuccess=false, string? failureMessage = null);
        //Task<IBuisnessLogicResult> AddLogoutAllSecurityEventRecordAsync();
        //Task<IBuisnessLogicResult> AddLogoutOthersSecurityEventRecordAsync();
        //Task<IBuisnessLogicResult> AddRefreshTokenSecurityEventRecordAsync();
        //Task<IBuisnessLogicResult> AddSignInSecurityEventRecordAsync();
        //Task<IBuisnessLogicResult> AddSignUpSecurityEventRecordAsync();
        //Task<IBuisnessLogicResult> AddVerifyOTPSecurityEventRecordAsync();


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

        public async Task<IBuisnessLogicResult> BlackListSessionTokensForASingleSessionAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("BlackListSessionTokensForASingleSessionAsync Started", accessToken);

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

                await LogDebugAsync("BlackListSessionTokensForASingleSessionAsync Completed", new { AccessToken = accessToken , UserUuid = userUuid, SessionUuid = sessionUuid });
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlackListSessionTokensForASingleSessionAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistSessionsExcludedByOneAsync(string accessToken)
        {
            try
            {
                await LogDebugAsync("BlacklistSessionsExcludedByOneAsync Started", accessToken);

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

                await LogDebugAsync("BlacklistSessionsExcludedByOneAsync Completed", new { AccessToken = accessToken, UserUuid = userUuid, CurrentSessionUuid = currentSessionUuid });
                return new BuisnessLogicSuccessResult("Access token blacklisted successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("BlacklistSessionsExcludedByOneAsync Excepted", ex, accessToken);
                return new BuisnessLogicErrorResult("Error blacklisting access token", 500);
            }
        }

        public async Task<IBuisnessLogicResult> BlacklistAllSessionTokensByUserAsync(string accessToken)
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
                        signInResponseDto.UserTypeId = (int)UserTypeId.Admin;
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
                        signInResponseDto.UserTypeId = (int)UserTypeId.Staff;
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
                        signInResponseDto.UserTypeId = (int)UserTypeId.Student;
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

        public async Task<IBuisnessLogicResult> CheckVerifyOTPAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto)
        {
            try
            {
                await LogDebugAsync("CheckVerifyOTPAsync Started", verifyOTPRequestDto);

                bool isExist = await _OTPCodeService.IsCodeExistAsync(verifyOTPRequestDto.SessionUuid.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.OtpTypeId.ToString(),
                    verifyOTPRequestDto.OtpCode);

                if (!isExist)
                {
                    return new BuisnessLogicErrorResult("OTP code is invalid or expired", 400);
                }

                bool isDeleted = await _OTPCodeService.RemoveCodeAsync(
                    verifyOTPRequestDto.SessionUuid.ToString(),
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.OtpTypeId.ToString(),
                    verifyOTPRequestDto.OtpCode);
                if (!isDeleted)
                {
                    return new BuisnessLogicErrorResult("Failed to delete OTP code", 500);
                }

                verifyOTPResponseDto.SessionUuid = verifyOTPRequestDto.SessionUuid;
                verifyOTPResponseDto.OtpTypeId = verifyOTPRequestDto.OtpTypeId;
                verifyOTPResponseDto.UserUuid = verifyOTPRequestDto.UserUuid;

                await LogDebugAsync("CheckVerifyOTP Completed", new
                {
                    UserType = verifyOTPRequestDto.UserTypeId,
                    SessionUuid = verifyOTPRequestDto.SessionUuid,
                    OtpTypeId = verifyOTPRequestDto.OtpTypeId,
                    OtpCode = verifyOTPRequestDto.OtpCode
                });
                return new BuisnessLogicSuccessResult("OTP code is valid", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("CheckVerifyOTPAsync Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult("CheckVerifyOTP işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> CreatSession(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto)
        {
            try
            {
                await LogDebugAsync("CreatSession Started", verifyOTPRequestDto);

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

                await LogDebugAsync("CreatSession Completed", new
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
                await LogErrorAsync("CreatSession Excepted", ex, verifyOTPRequestDto);
                return new BuisnessLogicErrorResult("CreatSession işlemi sırasında hata oluştu", 500);
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

        public async Task<IBuisnessLogicResult> AddSecurityEventRecordAsync<TKey, TValue>(HttpContext? httpContext, Guid securityEventTypeGuid, UserTypeId userTypeId, Guid userUuid, Guid? universityUuid, string description, Dictionary<TKey, TValue>? additionalData)
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

                SecurityEvent securityEvent = new SecurityEvent{
                    EventTypeUuid = securityEventTypeGuid,
                    UniversityUuid = universityUuid,
                    EventedByAdminUuid = userTypeId == UserTypeId.Admin ? userUuid : null,
                    EventedByStaffUuid = userTypeId == UserTypeId.Staff ? userUuid : null,
                    EventedByStudentUuid = userTypeId == UserTypeId.Student ? userUuid : null,
                    Description = description,
                    IpAddress = httpContext?.Connection?.RemoteIpAddress?.IsIPv4MappedToIPv6 == true
                        ? httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
    :                   httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown",
                    UserAgent = httpContext?.Request?.Headers["User-Agent"].ToString(),
                    AdditionalData = additionalData != null ? JsonSerializer.Serialize(additionalData) : null
                };

                bool isAdded = await _securityEventService.RecordSecurityEventAsync(securityEvent);
                if (!isAdded)
                {
                    return new BuisnessLogicErrorResult("Failed to add security event record", 500);
                }
                return new BuisnessLogicErrorResult("Security event record added successfully", 200);

            }
            catch (Exception ex)
            {
                await LogErrorAsync("AddSecurityEventRecordAsync Excepted", ex, new { securityEventTypeGuid, userTypeId, userUuid, description, additionalData });
                return new BuisnessLogicErrorResult("AddSecurityEventRecord işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> AddLogoutSecurityEventRecordAsync(HttpContext? httpContext, LogoutRequestDto logoutRequestDto, bool isEventSuccess = false, string? failureMessage = null)
        {
            // TODO: After Jwt Update this method should be updated to use the new JWT structure and logic.
            try
            {
                await LogDebugAsync("AddLogoutSecurityEventRecordAsync Started", new
                {
                    LogoutRequestDto = logoutRequestDto,
                    IsEventSuccess = isEventSuccess,
                    FailureMessage = failureMessage
                });

                var userUuid = await _sessionJwtService.GetUserUuidFromTokenAsync(logoutRequestDto.AccessToken);

                if (string.IsNullOrEmpty(userUuid))
                {
                    return new BuisnessLogicErrorResult("User UUID is not found on AccessToken", 400);
                }

                UserTypeId userTypeId = UserTypeId._;
                Guid securityEventUserUuid = Guid.Parse(userUuid);
                Guid? userUniversityUuid = null;

                Admin ? userAdmin = await _adminService.GetByUuidAsync(securityEventUserUuid);
                Staff? userStaff = await _staffService.GetByUuidAsync(securityEventUserUuid);
                Student? UserStudent = await _studentService.GetByUuidAsync(securityEventUserUuid);
                if (userAdmin != null)
                {
                    userTypeId = UserTypeId.Admin;
                    userUniversityUuid = userAdmin.UniversityUuid;
                }
                else if (userStaff != null)
                {
                    userTypeId = UserTypeId.Staff;
                    userUniversityUuid = userStaff.UniversityUuid;
                }
                else if (UserStudent != null)
                {
                    userTypeId = UserTypeId.Student;
                    userUniversityUuid = UserStudent.UniversityUuid;
                }
                else
                {
                    return new BuisnessLogicErrorResult("User not found", 404);
                }

                Guid securityEventTypeUuid = SecurityEventTypeGuids.EventGuids[SecurityEventTypeGuid.Logout];


                IBuisnessLogicResult result = await AddSecurityEventRecordAsync<object, object>(httpContext, securityEventTypeUuid, userTypeId, securityEventUserUuid, userUniversityUuid, "Logout", null);

                if (!result.Success)
                {
                    return new BuisnessLogicErrorResult("Failed to add logout security event record", 500);
                }

                await LogDebugAsync("AddLogoutSecurityEventRecordAsync Completed", new
                {
                    LogoutRequestDto = logoutRequestDto,
                    IsEventSuccess = isEventSuccess,
                    FailureMessage = failureMessage,
                    UserUuid = securityEventUserUuid,
                    UserTypeId = userTypeId,
                    UniversityUuid = userUniversityUuid
                });
                return new BuisnessLogicSuccessResult("Logout security event record added successfully", 200);
            }
            catch (Exception ex)
            {
                await LogErrorAsync("AddLogoutSecurityEventRecordAsync Excepted", ex, new { logoutRequestDto, isEventSuccess, failureMessage });
                return new BuisnessLogicErrorResult("AddLogoutSecurityEventRecord işlemi sırasında hata oluştu", 500);
            }
        }
    }
}
