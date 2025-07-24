using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using Buisness.Validators.FluentValidation.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.SignInValidators
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            Include(new UserTypeIdCarrierValidator<SignInCommand>());
            Include(new EmailOrPhoneCarrierValidator<SignInCommand>());
            Include(new UserUuidCarrierValidator<SignInCommand>());
            Include(new SessionUuidCarrierValidator<SignInCommand>());
            Include(new PasswordCarrierValidator<SignInCommand>());
        }

    }
}
