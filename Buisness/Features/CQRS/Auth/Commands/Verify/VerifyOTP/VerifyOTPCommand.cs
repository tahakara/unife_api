using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP
{
    public class VerifyOTPCommand : VerifyOTPRequestDto, ICommand<BaseResponse<VerifyOTPResponseDto>>
    {
    }
}
