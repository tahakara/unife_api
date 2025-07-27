using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Common;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail
{
    public class SendEmailVerificationOTPCommand : ICommand<BaseResponse<bool>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; }
    }

    public class VerifyEmailCommand : ICommand<BaseResponse<bool>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; }
        public string? OtpCode { get; set; }
    }

    public class SendEmailVerificationOTPCommandHandler : AuthCommandHandlerBase<SendEmailVerificationOTPCommand>, ICommandHandler<SendEmailVerificationOTPCommand, BaseResponse<bool>>
    {
        public SendEmailVerificationOTPCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SendEmailVerificationOTPCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "SendEmailVerificationOTP")
        {
        }

        public async Task<BaseResponse<bool>> Handle(SendEmailVerificationOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName));

                var httpContext = _httpContextAccessor.HttpContext;

                SendEmailVerificationOTPRequestDto verifyEmailGetRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                    });




                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    statusCode: 500);
            }
        }
    }

    public class VerifyEmailPostCommandHandler : AuthCommandHandlerBase<VerifyEmailCommand>, ICommandHandler<VerifyEmailCommand, BaseResponse<bool>>
    {
        public VerifyEmailPostCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<VerifyEmailCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "VerifyEmail")
        {
        }

        public async Task<BaseResponse<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug(message: CQRSLogMessages.ProccessStarted(_commandFullName));

                var httpContext = _httpContextAccessor.HttpContext;

                VerifyEmailRequestDto verifyEmailPostRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    new Func<StepContext, Task<IBuisnessLogicResult>>[]
                    {
                    });

                _logger.LogDebug(message: CQRSLogMessages.ProccessCompleted(_commandFullName));
                return BaseResponse<bool>.Success(
                    data: true,
                    message: CQRSResponseMessages.Success(_commandName),
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(CQRSLogMessages.ProccessFailed(_commandFullName, ex.Message));
                return BaseResponse<bool>.Failure(
                    message: CQRSResponseMessages.Error(_commandName),
                    statusCode: 500);
            }
        }
    }
}
