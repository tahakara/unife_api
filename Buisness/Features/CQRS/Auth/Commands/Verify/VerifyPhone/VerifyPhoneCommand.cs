using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyPhone
{
    // TODO: Will be implement after the sms service is ready.
    public class VerifyPhoneCommand : ICommand<BaseResponse<bool>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; }
    }
}
