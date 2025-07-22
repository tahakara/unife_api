using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword
{
    public class ChangePasswordCommand : ChangePasswordRequestDto, ICommand<BaseResponse<bool>>
    {
    }

    public class ChangePasswordCommandHandler : AuthCommandHandlerBase<ChangePasswordCommand>, ICommandHandler<ChangePasswordCommand, BaseResponse<bool>>
    {
        public ChangePasswordCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ChangePasswordCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("ChangePassword işlemi başlatıldı. UserType: {UserType} UserUuid: {UserUuid}", request.UserTypeId, request.UserUuid);

                var httpContext = _httpContextAccessor.HttpContext;

                ChangePasswordRequestDto changePasswordRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.ValidateAsync(request),
                    () => _authBusinessLogicHelper.MapToDtoAsync(request, changePasswordRequestDto));

                if (buisnessResult != null)
                {
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "ChangePassword işlemi sırasında hata oluştu",
                        statusCode: buisnessResult.StatusCode);
                }

                //IBuisnessLogicResult changePasswordResult = await BuisnessLogic.Run(
                //    () => _authBusinessLogicHelper.ChangePasswordAsync(request, mappedRequestDto));

                //if (changePasswordResult != null)
                //{
                //    return BaseResponse<bool>.Failure(
                //        message: changePasswordResult.Message ?? "ChangePassword işlemi sırasında hata oluştu",
                //        statusCode: changePasswordResult.StatusCode);
                //}
                
                _logger.LogInformation("ChangePassword işlemi başarılı. UserType: {UserType} UserUuid: {UserUuid}", request.UserTypeId, request.UserUuid);

                return BaseResponse<bool>.Success(
                    data: true,
                    message: "ChangePassword işlemi başarılı");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangePassword işlemi sırasında hata oluştu. UserType: {UserType} UserUuid: {UserUuid}", request.UserTypeId, request.UserUuid);
                return BaseResponse<bool>.Failure(
                    message: "ChangePassword işlemi sırasında hata oluştu",
                    errors : new List<string> { ex.Message },
                    statusCode: 500);
            }
        }
    }
}

