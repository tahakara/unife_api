using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP
{
    public class ResendSignInOTPCommand : SignInCommand, ICommand<BaseResponse<SignInResponseDto>>
    {
    }
}
