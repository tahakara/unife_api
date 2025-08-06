using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.DTOs.AuthDtos.SignUpDtos;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth.Base;
using Core.Enums;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.SignUp
{
    public class SignUpCommandHandler : AuthCommandHandlerBase<SignUpCommand>, ICommandHandler<SignUpCommand, BaseResponse<SignUpResponseDto>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<SignUpCommand> _logger;

        public SignUpCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SignUpCommand> logger) : base(authBusinessLogicHelper, httpContextAccessor, logger, "SignUp")
        {
        }

        public async Task<BaseResponse<SignUpResponseDto>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName));

                var httpContext = _httpContextAccessor.HttpContext;

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
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.SignUpFailed,
                        methodName: nameof(SignUpCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogDebug(message: CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<SignUpResponseDto>.Failure(
                        message: buisnessResult.Message ?? "SignUp işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.SignUpSucceeded,
                    methodName: nameof(SignUpCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: null,
                    isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<SignUpResponseDto>.Success(
                    data: signUpResponsetDto,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request));
                return BaseResponse<SignUpResponseDto>.Failure(
                    message: CQRSResponseMessages.Fail(_commandName, ex.Message),
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}
