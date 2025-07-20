using AutoMapper;
using Buisness.Abstract.DtoBase.Base;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
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
using DataAccess.Abstract;
using DataAccess.Enums;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using FluentValidation;
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
    public interface IAuthBuisnessLogicHelper : IBuisnessLogicHelper
    {
        Task<IBuisnessLogicResult> ValidateCommandAsync<T>(T command) where T : class, new();
        Task<IBuisnessLogicResult> MapToDtoAsync<TSource, TDestination>(TSource source, TDestination target)
            where TSource : class, new()
            where TDestination : class, new();
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
        private readonly IMapper _mapper;


        public AuthBuisnessLogicHelper(
            ISessionJwtService sessionJwtService,
            IOTPCodeService OTPCodeService,
            IEmailService emailService,
            IPasswordUtility passwordUtility,
            IOTPUtilitiy otpUtilitiy,
            IAdminService adminService,
            IStaffService staffService,
            IStudentService studentService,
            IMapper mapper,
            ILogger<AuthBuisnessLogicHelper> logger,
            IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            _sessionJwtService = sessionJwtService;
            _OTPCodeService = OTPCodeService;
            _emailService = emailService;
            _passwordUtility = passwordUtility;
            _otpUtilitiy = otpUtilitiy;
            _adminService = adminService;
            _staffService = staffService;
            _studentService = studentService;
            _mapper = mapper;
            //_logger = logger;
        }

        public async Task<IBuisnessLogicResult> ValidateCommandAsync<T>(T command) where T : class, new()
        {
            try
            {
                _logger.LogDebug("Validation started for {DtoType}", typeof(T).Name);
                if (command == null)
                {
                    return new BuisnessLogicErrorResult("Validation failed: DTO is null", 400);
                }

                await ValidateAsync(command);

                _logger.LogDebug("Validation successful for {DtoType}", typeof(T).Name);
                return new BuisnessLogicSuccessResult("Validation successful", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Validation failed for {DtoType}", typeof(T).Name);
                return new BuisnessLogicErrorResult("Validation failed", 500);
            }
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

        public async Task<IBuisnessLogicResult> CheckAndCreateSignUpCredentialsAsync(SignUpRequestDto signUpRequestDto, SignUpResponseDto signUpResponseDto)
        {
            try
            {
                _logger.LogDebug("CheckSignUpCredentials işlemi başlatıldı. UserTypeId: {UserTypeId}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    signUpRequestDto.UserTypeId, signUpRequestDto.Email, signUpRequestDto.PhoneCountryCode, signUpRequestDto.PhoneNumber);


                switch (signUpRequestDto.UserTypeId)
                {
                    case (int)UserTypeId.Admin:
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

                    case (int)UserTypeId.Staff:
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
                    case (int)UserTypeId.Student:

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
                _logger.LogError(ex, "CheckSignUpCredentials işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("CheckSignUpCredentials işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> CheckSignInCredentialsAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto)
        {
            try
            {
                _logger.LogDebug("CheckSignInCredentials işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    signInRequestDto.UserTypeId, signInRequestDto.Email, signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);

                switch (signInRequestDto.UserTypeId)
                {
                    case (int)UserTypeId.Admin:
                        Admin? currentAdmin = new();

                        if (!(string.IsNullOrEmpty(signInRequestDto.Email) && string.IsNullOrEmpty(signInRequestDto.Password)))
                        {
                            currentAdmin = await _adminService.GetAdminByEmailAsync(signInRequestDto.Email);
                        }
                        else if (!((string.IsNullOrEmpty(signInRequestDto.PhoneCountryCode) && string.IsNullOrEmpty(signInRequestDto.PhoneNumber)) &&
                            string.IsNullOrEmpty(signInRequestDto.Password)))
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

                    case (int)UserTypeId.Staff:
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

                    case (int)UserTypeId.Student:
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
                _logger.LogDebug("CheckSignInCredentials işlemi başarılı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    signInRequestDto.UserTypeId, signInRequestDto.Email, signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);
                return new BuisnessLogicSuccessResult("SignIn credentials are valid", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckSignInCredentials işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("CheckSignInCredentials işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> SendSignInOTPAsync(SignInRequestDto signInRequestDto, SignInResponseDto signInResponseDto)
        {
            try
            {
                _logger.LogDebug("SendSignInOTP işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    signInRequestDto.UserTypeId, signInRequestDto.Email, signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);

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

                _logger.LogDebug("SendSignInOTP işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    signInRequestDto.UserTypeId, signInRequestDto.Email, signInRequestDto.PhoneCountryCode, signInRequestDto.PhoneNumber);
                return new BuisnessLogicSuccessResult("SendSignInOTP işlemi başarılı", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendSignInOTP işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("SendSignInOTP işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> CheckVerifyOTPAsync(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto)
        {
            try
            {
                _logger.LogDebug("CheckVerifyOTP işlemi başlatıldı. UserType: {UserType}, SessionUuid: {SessionUuid}, OtpTypeId: {OtpTypeId}, OtpCode: {OtpCode}",
                    verifyOTPRequestDto.UserTypeId, verifyOTPRequestDto.SessionUuid, verifyOTPRequestDto.OtpTypeId, verifyOTPRequestDto.OtpCode);

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

                return new BuisnessLogicSuccessResult("OTP code is valid", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckVerifyOTP işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("CheckVerifyOTP işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> CreatSession(VerifyOTPRequestDto verifyOTPRequestDto, VerifyOTPResponseDto verifyOTPResponseDto)
        {
            try
            {
                _logger.LogDebug("CreatSession işlemi başlatıldı. UserType: {UserType}, SessionUuid: {SessionUuid}, OtpTypeId: {OtpTypeId}, OtpCode: {OtpCode}",
                    verifyOTPRequestDto.UserTypeId, verifyOTPRequestDto.SessionUuid, verifyOTPRequestDto.OtpTypeId, verifyOTPRequestDto.OtpCode);

                string newAccessTOken = await _sessionJwtService.GenerateAccessTokenAsync(
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.SessionUuid.ToString());

                string newRefreshToken = await _sessionJwtService.GenerateRefreshTokenAsync(
                    verifyOTPRequestDto.UserUuid.ToString(),
                    verifyOTPRequestDto.SessionUuid.ToString());

                verifyOTPResponseDto.AccessToken = newAccessTOken;
                verifyOTPResponseDto.RefreshToken = newRefreshToken;

                return new BuisnessLogicSuccessResult("Session created successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreatSession işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("CreatSession işlemi sırasında hata oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> RevokeOldOTPAsync(SignInRequestDto signInRequestDto)
        {
            try
            {
                _logger.LogDebug("RevokeOldOTP işlemi başlatıldı. UserType: {UserType}, SessionUuid: {SessionUuid}, OtpTypeId: {OtpTypeId}",
                    signInRequestDto.UserTypeId, signInRequestDto.SessionUuid, signInRequestDto.OtpTypeId);
                bool isRevoked = await _OTPCodeService.RevokeCodeByUserUuid(signInRequestDto.UserUuid.ToString());
                if (!isRevoked)
                {
                    return new BuisnessLogicErrorResult("Failed to revoke old OTP", 500);
                }
                _logger.LogDebug("RevokeOldOTP işlemi başarılı. UserType: {UserType}, SessionUuid: {SessionUuid}, OtpTypeId: {OtpTypeId}",
                    signInRequestDto.UserTypeId, signInRequestDto.SessionUuid, signInRequestDto.OtpTypeId);
                return new BuisnessLogicSuccessResult("RevokeOldOTP işlemi başarılı", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RevokeOldOTP işlemi sırasında hata oluştu");
                return new BuisnessLogicErrorResult("RevokeOldOTP işlemi sırasında hata oluştu", 500);
            }
        }
    }
}
