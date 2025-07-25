using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
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
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "SignIn")
        {
        }

        public async Task<BaseResponse<SignInResponseDto>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {

            try
            {
                //_logger.LogDebug("SignIn işlemi başlatıldı. UserType: {UserType}, Email: {Email}, PhoneCountryCode: {PhoneCountryCode}, PhoneNumber: {PhoneNumber}",
                    //request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber);

                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName, new { request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber }));
                var httpContext = _httpContextAccessor.HttpContext;

                SignInRequestDto signInRequestDto = new();
                SignInResponseDto signInResponseDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, signInRequestDto),
                        ctx => _authBusinessLogicHelper.PreventSignInBruteForceAsync(signInRequestDto),
                        ctx => _authBusinessLogicHelper.RevokeOldOTPAsync(signInRequestDto),
                        ctx => _authBusinessLogicHelper.CheckSignInCredentialsAsync(signInRequestDto, signInResponseDto),
                        ctx => _authBusinessLogicHelper.CheckUserSessionCountExceededAsync(signInResponseDto),

                        // Executors
                        ctx => _authBusinessLogicHelper.SendSignInOTPAsync(signInRequestDto, signInResponseDto)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSignInOTPResendEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.LoginFailure,
                        methodName: nameof(SignInCommandHandler),
                        description: _commandFullName,
                        userGuid: signInResponseDto.UserUuid,
                        userTypeId: signInResponseDto.UserTypeId,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    return BaseResponse<SignInResponseDto>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSignInOTPResendEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.LoginSuccess,
                        methodName: nameof(SignInCommandHandler),
                        description: _commandFullName,
                        userGuid: signInResponseDto.UserUuid,
                        userTypeId: signInResponseDto.UserTypeId,
                        accessToken: null,
                        isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName, new { request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber }));
                return BaseResponse<SignInResponseDto>.Success(
                    data: signInResponseDto,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, new { request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber }));
                return BaseResponse<SignInResponseDto>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}
