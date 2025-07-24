using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP
{
    public class ResendSignInOTPCommandHandler : AuthCommandHandlerBase<ResendSignInOTPCommand>, ICommandHandler<ResendSignInOTPCommand, BaseResponse<SignInResponseDto>>
    {
        public ResendSignInOTPCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ResendSignInOTPCommand> logger) 
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<SignInResponseDto>> Handle(ResendSignInOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("SignIn işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);
                var httpContext = _httpContextAccessor.HttpContext;

                SignInRequestDto signInRequestDto = new();
                SignInResponseDto signInResponseDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, signInRequestDto),
                        ctx =>  _authBusinessLogicHelper.CheckSignInCredentialsAsync(signInRequestDto, signInResponseDto),
                        ctx => _authBusinessLogicHelper.RevokeOldOTPAsync(signInRequestDto),

                        // Executors
                        ctx => _authBusinessLogicHelper.SendSignInOTPAsync(signInRequestDto, signInResponseDto)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddResendSignInOTPSecurityEventRecordAsync(
                        httpContext, 
                        SecurityEventTypeGuid.VerificationOTPResend, 
                        nameof(ResendSignInOTPCommandHandler), 
                        "Resnd SignIn OTP", 
                        signInResponseDto.UserUuid, 
                        signInResponseDto.UserTypeId, 
                        null, 
                        false, 
                        buisnessResult.Message ?? "ResendSignInOTP işlemi sırasında hata oluştu");

                    return BaseResponse<SignInResponseDto>.Failure(
                        message: buisnessResult.Message ?? "ResendSignInOTP işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddResendSignInOTPSecurityEventRecordAsync(
                    httpContext, 
                    SecurityEventTypeGuid.VerificationOTPResend, 
                    nameof(ResendSignInOTPCommandHandler), 
                    "Resnd SignIn OTP", 
                    signInResponseDto.UserUuid, 
                    signInResponseDto.UserTypeId, 
                    null, 
                    true, 
                    "ResendSignInOTP işlemi başarılı");
                _logger.LogDebug("ResendSignInOTP işlemi başarılı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);
                return BaseResponse<SignInResponseDto>.Success(
                    data: signInResponseDto,
                    message: "SignIn işlemi başarılı",
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ResendSignInOTP işlemi sırasında hata oluştu");
                return BaseResponse<SignInResponseDto>.Failure(
                    message: "SignIn işlemi sırasında hata oluştu",
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}