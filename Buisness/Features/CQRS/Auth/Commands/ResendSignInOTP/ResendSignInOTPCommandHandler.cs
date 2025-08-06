using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.DTOs.AuthDtos.SignInDtos;
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
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "ResendSignInOTP")
        {
        }

        public async Task<BaseResponse<SignInResponseDto>> Handle(ResendSignInOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName, new { request.UserTypeId, request.Email, request.PhoneCountryCode, request.PhoneNumber }));
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
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.VerificationResendFailed,
                        methodName: nameof(ResendSignInOTPCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogDebug(message: CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<SignInResponseDto>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.VerificationResendSucceeded,
                    methodName: nameof(RefreshTokenCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: null,
                    isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<SignInResponseDto>.Success(
                    data: signInResponseDto,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request));
                return BaseResponse<SignInResponseDto>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    errors: new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}