using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP
{
    public class VerifyOTPCommandHandler : AuthCommandHandlerBase<VerifyOTPCommand>, ICommandHandler<VerifyOTPCommand, BaseResponse<VerifyOTPResponseDto>>
    {
        public VerifyOTPCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VerifyOTPCommand> logger) 
            : base (authBusinessLogicHelper, httpContextAccessor, logger, "VerifyOTP")
        {
        }

        public async Task<BaseResponse<VerifyOTPResponseDto>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName, new { request.UserTypeId, request.UserUuid }));

                var httpContext = _httpContextAccessor.HttpContext;

                VerifyOTPRequestDto verifyOTPRequestDto = new();
                VerifyOTPResponseDto verifyOTPResponseDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                        ctx => _authBusinessLogicHelper.ValidateAsync(request),
                        ctx => _authBusinessLogicHelper.MapToDtoAsync(request, verifyOTPRequestDto),
                        ctx => _authBusinessLogicHelper.CheckVerifyOTPAsync(verifyOTPRequestDto, verifyOTPResponseDto),

                        // Executors
                        ctx => _authBusinessLogicHelper.CreateSessionAsync(verifyOTPRequestDto, verifyOTPResponseDto),
                        ctx => _authBusinessLogicHelper.SignInCompletedAsync(verifyOTPRequestDto, httpContext),
                        
                        // FullSuccess
                        ctx => _authBusinessLogicHelper.RevokeSignInBruteForceTokenAsync(verifyOTPRequestDto.UserTypeId, userUuid: verifyOTPRequestDto.UserUuid)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                        httpContext: httpContext,
                        eventTypeGuidKey: SecurityEventTypeGuid.VerificationSignInOTPFailed,
                        methodName: nameof(ResendSignInOTPCommandHandler),
                        description: _commandFullName,
                        userGuid: null,
                        userTypeId: UserTypeId._,
                        accessToken: null,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown);

                    _logger.LogDebug(CQRSLogMessages.ProccessFailed(_commandFullName, buisnessResult.Message));
                    return BaseResponse<VerifyOTPResponseDto>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }


                await _authBusinessLogicHelper.AddSecurityEventRecordAsync(
                    httpContext: httpContext,
                    eventTypeGuidKey: SecurityEventTypeGuid.VerificationSignInOTPSucceeded,
                    methodName: nameof(VerifyOTPCommandHandler),
                    description: _commandFullName,
                    userGuid: null,
                    userTypeId: UserTypeId._,
                    accessToken: null,
                    isEventSuccess: true);

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<VerifyOTPResponseDto>.Success(
                    data: verifyOTPResponseDto,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, request));
                return BaseResponse<VerifyOTPResponseDto>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    statusCode: 500);
            }
        }
    }
}
