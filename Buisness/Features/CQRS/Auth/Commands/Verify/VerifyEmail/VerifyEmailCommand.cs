using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail
{
    public class VerifyEmailCommand : TokenCommandBase, ICommand<BaseResponse<bool>>
    {
    }
}
