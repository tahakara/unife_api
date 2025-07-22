using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
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
            : base (authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<VerifyOTPResponseDto>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("VerifyOTP işlemi başlatıldı. UserType: {UserType}");

                VerifyOTPRequestDto verifyOTPRequestDto = new();
                VerifyOTPResponseDto verifyOTPResponseDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, verifyOTPRequestDto),
                    await _authBusinessLogicHelper.CheckVerifyOTPAsync(verifyOTPRequestDto, verifyOTPResponseDto)
                );
                if (buisnessResult != null)
                    return BaseResponse<VerifyOTPResponseDto>.Failure(
                        message: buisnessResult.Message ?? "VerifyOTP işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);


                IBuisnessLogicResult createBuisnessLogicResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.CreatSession(verifyOTPRequestDto, verifyOTPResponseDto));
                if (createBuisnessLogicResult != null)
                {
                    await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                        _httpContextAccessor.HttpContext,
                        verifyOTPResponseDto.AccessToken,
                        SecurityEventTypeGuid.VerificationOTPFailed,
                        nameof(VerifyOTPCommandHandler),
                        "VerifyOTP",
                        false,
                        createBuisnessLogicResult.Message ?? "VerifyOTP işlemi sırasında hata oluştu"
                    );
                    return BaseResponse<VerifyOTPResponseDto>.Failure(
                        message: buisnessResult.Message ?? "VerifyOTP işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

                await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    _httpContextAccessor.HttpContext,
                    verifyOTPResponseDto.AccessToken,
                    SecurityEventTypeGuid.VerificationOTPSuccess,
                    nameof(VerifyOTPCommandHandler),
                    "VerifyOTP",
                    true,
                    "VerifyOTP işlemi başarılı"
                );
                _logger.LogDebug("VerifyOTP işlemi başarılı. UserType: {UserType}", request.UserTypeId);
                return BaseResponse<VerifyOTPResponseDto>.Success(
                    data: verifyOTPResponseDto,
                    message: "VerifyOTP işlemi başarılı",
                    statusCode: 200);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VerifyOTP işlemi sırasında hata oluştu");
                return BaseResponse<VerifyOTPResponseDto>.Failure(
                    message: "VerifyOTP işlemi sırasında hata oluştu",
                    statusCode: 500);
            }
        }
    }
}
