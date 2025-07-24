using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
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
                        ctx => _authBusinessLogicHelper.CreatSession(verifyOTPRequestDto, verifyOTPResponseDto),
                        ctx => _authBusinessLogicHelper.SiginCompletedAsync(verifyOTPRequestDto, httpContext)
                    });

                if (buisnessResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        httpContext: httpContext,
                        accessToken: verifyOTPResponseDto.AccessToken,
                        eventTypeGuidKey: SecurityEventTypeGuid.VerificationOTPFailed,
                        methodName: nameof(VerifyOTPCommandHandler),
                        description: _commandFullName,
                        isEventSuccess: false,
                        failureMessage: buisnessResult.Message ?? CQRSLogMessages.Unknown
                    );
                    return BaseResponse<VerifyOTPResponseDto>.Failure(
                        message: CQRSResponseMessages.Fail(_commandName, buisnessResult.Message),
                        statusCode: buisnessResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    httpContext: httpContext,
                    accessToken: verifyOTPResponseDto.AccessToken,
                    eventTypeGuidKey: SecurityEventTypeGuid.VerificationOTPSuccess,
                    methodName: nameof(VerifyOTPCommandHandler),
                    description: _commandFullName,
                    isEventSuccess: true
                );

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName, new { request.UserTypeId, request.UserUuid }));
                return BaseResponse<VerifyOTPResponseDto>.Success(
                    data: verifyOTPResponseDto,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message, new { request.UserTypeId, request.UserUuid }));
                return BaseResponse<VerifyOTPResponseDto>.Failure(
                    message: CQRSResponseMessages.Fail(_commandName, ex.Message),
                    statusCode: 500);
            }
        }
    }
}
