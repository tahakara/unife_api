using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail
{

    public class VerifyEmailCommand : ICommand<BaseResponse<bool>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; }
        public string? OtpCode { get; set; }
    }
}
