using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Auth.Commands.LogoutOthers;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.LogoutOthers
{
    public class LogoutOthersCommand : LogoutCommand, ICommand<BaseResponse<bool>>
    {
    }
}
