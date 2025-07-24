using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class UserUuidCarrierValidator<T> : AbstractValidator<T>
    where T : IUserUuidCarrier
    {
        public UserUuidCarrierValidator()
        {
            RuleFor(x => x.UserUuid)
                .NotNull()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IUserUuidCarrier.UserUuid)))
                .NotEmpty()
                    .WithMessage(ValidationMessage.NotEmptyFormat(nameof(IUserUuidCarrier.UserUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessage.InvalidFormat(nameof(IUserUuidCarrier.UserUuid)));
        }
    }
}
