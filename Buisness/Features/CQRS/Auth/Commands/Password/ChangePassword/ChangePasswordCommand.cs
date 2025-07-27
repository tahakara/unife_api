using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword
{
    public class ChangePasswordCommand : 
        IAccessTokenCarrier, 
        INewPasswordCarrier,
        ICommand<BaseResponse<bool>>
    {
        public string? AccessToken { get; set; } = null;
        public string? OldPassword { get; set; } = null;
        public string? NewPassword { get; set; } = null;
        public string? ConfirmPassword { get; set; } = null;
        public bool LogoutOtherSessions { get; set; } = true;
    }
}

