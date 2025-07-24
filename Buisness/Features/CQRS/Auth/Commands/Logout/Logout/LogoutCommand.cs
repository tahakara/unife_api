using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Logout.Logout
{
    public class LogoutCommand : ICommand<BaseResponse<bool>>, 
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; } = null;
    }

}
