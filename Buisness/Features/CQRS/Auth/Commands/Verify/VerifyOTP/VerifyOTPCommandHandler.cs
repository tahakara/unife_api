using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP
{
    public class VerifyOTPCommandHandler : ICommandHandler<VerifyOTPCommand, BaseResponse<VerifyOTPResponseDto>>
    {
        private readonly IAuthBuisnessLogicHelper _authBusinessLogicHelper;
        private readonly ILogger<VerifyOTPCommandHandler> _logger;

        public VerifyOTPCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            ILogger<VerifyOTPCommandHandler> logger)
        {
            _authBusinessLogicHelper = authBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<VerifyOTPResponseDto>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("VerifyOTP işlemi başlatıldı. UserType: {UserType}");

                VerifyOTPRequestDto verifyOTPRequestDto = new();
                VerifyOTPResponseDto verifyOTPResponseDto = new();

                IBuisnessLogicResult buisnessResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.ValidateCommandAsync(request),
                    await _authBusinessLogicHelper.MapToDtoAsync(request, verifyOTPRequestDto),
                    await _authBusinessLogicHelper.CheckVerifyOTPAsync(verifyOTPRequestDto, verifyOTPResponseDto)
                    
                );

                if (buisnessResult != null)
                {
                    return BaseResponse<VerifyOTPResponseDto>.Failure(
                        message: buisnessResult.Message ?? "VerifyOTP işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

                IBuisnessLogicResult createBuisnessLogicResult = BuisnessLogic.Run(
                    await _authBusinessLogicHelper.CreatSession(verifyOTPRequestDto, verifyOTPResponseDto));

                if (createBuisnessLogicResult != null)
                {
                    return BaseResponse<VerifyOTPResponseDto>.Failure(
                        message: buisnessResult.Message ?? "VerifyOTP işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

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
