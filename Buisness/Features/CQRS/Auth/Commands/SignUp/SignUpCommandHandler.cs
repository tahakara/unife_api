using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.SignUp
{
    public class SignUpCommandHandler : ICommandHandler<SignUpCommand, BaseResponse<SignUpResponseDto>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<SignUpCommand> _logger;

        public SignUpCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            ILogger<SignUpCommand> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<SignUpResponseDto>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("SignUp işlemi başlatıldı. UserTypeId: {UserTypeId}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);

                SignUpRequestDto signUpRequestDto = new();
                SignUpResponseDto signUpResponsetDto = new();

                IBuisnessLogicResult buisnessLogicResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.ValidateAsync(request),
                    () => _authBusinessLogicHelper.MapToDtoAsync(request, signUpRequestDto)                    
                );
                if (buisnessLogicResult != null)
                    return BaseResponse<SignUpResponseDto>.Failure(
                        message: buisnessLogicResult.Message ?? "SignUp işlemi sırasında hata oluştu",
                        statusCode: buisnessLogicResult.StatusCode);


                IBuisnessLogicResult signUpResult = await BuisnessLogic.Run(
                  () => _authBusinessLogicHelper.CheckAndCreateSignUpCredentialsAsync(signUpRequestDto, signUpResponsetDto));
                if (signUpResult != null)
                    return BaseResponse<SignUpResponseDto>.Failure(
                        message: signUpResult.Message ?? "SignUp işlemi sırasında hata oluştu",
                        statusCode: signUpResult.StatusCode);


                _logger.LogDebug("SignUp işlemi başarılı. UserTypeId: {UserTypeId}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);
                return BaseResponse<SignUpResponseDto>.Success(
                    data: signUpResponsetDto,
                    message: "SignUp işlemi başarılı",
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignUp işlemi sırasında hata oluştu");
                return BaseResponse<SignUpResponseDto>.Failure(
                    message: "SignUp işlemi sırasında hata oluştu",
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}
