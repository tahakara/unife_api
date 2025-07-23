using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
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
            return BaseResponse<bool>.Failure(
                    message: "An error occurred while processing your request.",
                    statusCode: 500);
        }
    }
}
