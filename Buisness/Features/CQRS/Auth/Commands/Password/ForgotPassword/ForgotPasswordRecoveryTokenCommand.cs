using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword
{
    public class ForgotPasswordRecoveryTokenCommand : ForgotPasswordRecoveryTokenRequestDto, ICommand<BaseResponse<bool>>
    {
    }
    public class ForgotPasswordRecoveryTokenCommandHandler : AuthCommandHandlerBase<ForgotPasswordRecoveryTokenCommand>, ICommandHandler<ForgotPasswordRecoveryTokenCommand, BaseResponse<bool>>
    {
        public ForgotPasswordRecoveryTokenCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ForgotPasswordRecoveryTokenCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger)
        {
        }

        public async Task<BaseResponse<bool>> Handle(ForgotPasswordRecoveryTokenCommand request, CancellationToken cancellationToken)
        {
            // TODO: Burada Kaldık
            try
            {
                _logger.LogDebug("Processing ForgotPasswordRecoveryTokenCommand with RecoveryToken: {RecoveryToken}", request.RecoveryToken);

                var httpContext = _httpContextAccessor.HttpContext;

                ForgotPasswordRecoveryTokenRequestDto forgotPasswordRecoveryTokenRequestDto = new();

                IBuisnessLogicResult buisnessResult = await BuisnessLogic.Run(
                    () => _authBusinessLogicHelper.ValidateAsync(request),
                    () => _authBusinessLogicHelper.MapToDtoAsync(request, forgotPasswordRecoveryTokenRequestDto),
                    () => _authBusinessLogicHelper.CheckRecoveryToken(forgotPasswordRecoveryTokenRequestDto),
                    () => _authBusinessLogicHelper.ResetUserPassword(forgotPasswordRecoveryTokenRequestDto));

                if (buisnessResult != null)
                {
                    //await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                    //    httpContext,
                    //    string.Empty,
                    //    SecurityEventTypeGuid.PasswordResetSuccess,
                    //    nameof(ForgotPasswordRecoveryTokenCommandHandler),
                    //    "Forgot Password Recovery Token",
                    //    false,
                    //    buisnessResult.Message ?? "An error occurred while processing your request.");
                    _logger.LogDebug("Business logic validation failed: {Message}", buisnessResult.Message);
                    return BaseResponse<bool>.Failure(
                        message: buisnessResult.Message ?? "An error occurred while processing your request.",
                        statusCode: buisnessResult.StatusCode);
                }

                //await _authBusinessLogicHelper.AddSecurityEventRecordByTypeAsync(
                //    httpContext,
                //    string.Empty,
                //    SecurityEventTypeGuid.PasswordResetSuccess,
                //    nameof(ForgotPasswordRecoveryTokenCommandHandler),
                //    "Forgot Password Recovery Token",
                //    true,
                //    buisnessResult.Message ?? "Recovery token processed successfully.");
                _logger.LogDebug("ForgotPasswordRecoveryTokenCommand completed successfully.");
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "Recovery token processed successfully.",
                    statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the ForgotPasswordRecoveryTokenCommand.");
                return BaseResponse<bool>.Failure(
                    message: "An error occurred while processing your request.",
                    statusCode: 500);
            }
        }
    }
}
