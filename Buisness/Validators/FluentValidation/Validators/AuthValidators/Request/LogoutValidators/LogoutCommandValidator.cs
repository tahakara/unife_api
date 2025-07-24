using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutOthers;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.LogoutValidators
{
    public class LogoutCommandValidator :
        AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator() 
        {
            Include(new AccessTokenCarrierValidator<LogoutCommand>());
        }
    }


    public class LogoutOthersCommandValidator :
        AbstractValidator<LogoutOthersCommand>
    {
        public LogoutOthersCommandValidator() 
        { 
            Include(new AccessTokenCarrierValidator<LogoutOthersCommand>());
        }
    }


    public class LogoutAllCommandValidator :
        AbstractValidator<LogoutAllCommand>
    {
        public LogoutAllCommandValidator() 
        {
            Include(new AccessTokenCarrierValidator<LogoutAllCommand>());
        }
    }
}
