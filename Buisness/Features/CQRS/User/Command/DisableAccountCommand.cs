using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Helpers.BuisnessLogicHelpers.Auth.Base;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.User.Command
{
    public class DisableAccountCommand : ICommand<BaseResponse<bool>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; } = null;
    }

    public class DisableAccountCommandHandler : AuthCommandHandlerBase<DisableAccountCommand>, ICommandHandler<DisableAccountCommand, BaseResponse<bool>>
    {
        public DisableAccountCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<DisableAccountCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "DisableAccount")
        {
        }

        public async Task<BaseResponse<bool>> Handle(DisableAccountCommand request, CancellationToken cancellationToken)
        {
            return BaseResponse<bool>.Success(
                data: true,
                message: "Account disabled successfully",
                statusCode: 200);
        }
    }
}
