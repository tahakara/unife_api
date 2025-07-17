using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.LogoutAll
{
    public class LogoutAllCommand : LogoutCommand, ICommand<BaseResponse<bool>>
    {
    }
}
