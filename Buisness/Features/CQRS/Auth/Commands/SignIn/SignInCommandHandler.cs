using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.SignIn
{
    public class SignInCommandHandler : AuthCommandHandlerBase<SignInCommand>, ICommandHandler<SignInCommand, BaseResponse<SignInResponseDto>>
    {
        public SignInCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SignInCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<SignInResponseDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogDebug("SignIn işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);

                var httpContext = _httpContextAccessor.HttpContext;

                SignInRequestDto signInRequestDto = new();
                SignInResponseDto signInResponseDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, signInRequestDto),
                    await _authBusinessLogicHelper.CheckSignInCredentialsAsync(signInRequestDto, signInResponseDto)
                );
                if (buisnessResult != null)
                    return BaseResponse<SignInResponseDto>.Failure(
                        message: buisnessResult.Message ?? "SignIn işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);


                IBuisnessLogicResult sendSignInResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.SendSignInOTPAsync(signInRequestDto, signInResponseDto));
                if (sendSignInResult != null)
                {
                    await _authBusinessLogicHelper.AddResendSignInOTPSecurityEventRecordAsync(
                        httpContext,
                        SecurityEventTypeGuid.LoginFailure,
                        nameof(SignInCommandHandler),
                        "SignIn",
                        signInResponseDto.UserUuid,
                        signInResponseDto.UserTypeId ?? 0,
                        null,
                        false,
                        sendSignInResult.Message ?? "SignIn işlemi sırasında hata oluştu");
                    return BaseResponse<SignInResponseDto>.Failure(
                        message: sendSignInResult.Message ?? "SignIn işlemi sırasında hata oluştu",
                        statusCode: sendSignInResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddResendSignInOTPSecurityEventRecordAsync(
                        httpContext,
                        SecurityEventTypeGuid.LoginSuccess,
                        nameof(SignInCommandHandler),
                        "SignIn",
                        signInResponseDto.UserUuid,
                        signInResponseDto.UserTypeId ?? 0,
                        null,
                        true);
                _logger.LogDebug("SignIn işlemi başarılı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);
                return BaseResponse<SignInResponseDto>.Success(
                    data: signInResponseDto,
                    message: "SignIn işlemi başarılı",
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignIn işlemi sırasında hata oluştu");
                return BaseResponse<SignInResponseDto>.Failure(
                    message: "SignIn işlemi sırasında hata oluştu",
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}
