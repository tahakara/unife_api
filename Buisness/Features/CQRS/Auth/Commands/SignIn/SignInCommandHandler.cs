using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.SignIn
{
    public class SignInCommandHandler : ICommandHandler<SignInCommand, BaseResponse<SignInResponseDto>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<SignInCommand> _logger;

        public SignInCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            ILogger<SignInCommand> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<SignInResponseDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
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
                    await _authBusinessLogicHelper.SendSignInOTPAsync(signInRequestDto, signInResponseDto) // TODO: OTP ,
                    );

                if (buisnessResult != null)
                {
                    return BaseResponse<SignInResponseDto>.Failure(
                        message: buisnessResult.Message ?? "SignIn işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

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
