using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP
{
    public class VerifyOTPCommand : ICommand<BaseResponse<VerifyOTPResponseDto>>,
        IUserTypeIdCarrier,
        IUserUuidCarrier,
        ISessionUuidCarrier
    {
        public byte? UserTypeId { get; set; } = null;
        public string? UserUuid { get; set; } = null;
        public string? SessionUuid { get; set; } = null;
        public byte? OtpTypeId { get; set; } = null;
        public string? OtpCode { get; set; } = null;
    }
}
