using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP
{
    public class ResendSignInOTPCommandHandler : ICommandHandler<ResendSignInOTPCommand, BaseResponse<SignInResponseDto>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<ResendSignInOTPCommand> _logger;

        public ResendSignInOTPCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            ILogger<ResendSignInOTPCommand> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<SignInResponseDto>> Handle(ResendSignInOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("SignIn işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);

                SignInRequestDto signInRequestDto = new();
                SignInResponseDto signInResponseDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateCommandAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, signInRequestDto),
                    await _authBusinessLogicHelper.CheckSignInCredentialsAsync(signInRequestDto, signInResponseDto),
                    await _authBusinessLogicHelper.RevokeOldOTPAsync(signInRequestDto)
                );

                if (buisnessResult != null)
                    return BaseResponse<SignInResponseDto>.Failure(
                        message: buisnessResult.Message ?? "ResendSignInOTP işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);


                IBuisnessLogicResult sendOtpResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.SendSignInOTPAsync(signInRequestDto, signInResponseDto));
                if (sendOtpResult != null)
                    return BaseResponse<SignInResponseDto>.Failure(
                        message: sendOtpResult.Message ?? "ResendSignInOTP işlemi sırasında hata oluştu",
                        statusCode: sendOtpResult.StatusCode);


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