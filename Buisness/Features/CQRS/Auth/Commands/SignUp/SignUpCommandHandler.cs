using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
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

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, signUpRequestDto),

                        // Executors
                        ctx => _authBusinessLogicHelper.CheckAndCreateSignUpCredentialsAsync(signUpRequestDto, signUpResponsetDto)
                    });
              
                if (buisnessResult != null)
                    return BaseResponse<SignUpResponseDto>.Failure(
                        message: buisnessResult.Message ?? "SignUp işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);


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
