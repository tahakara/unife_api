using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IUserUuidCarrier.UserUuid)))
                .NotEmpty()
                    .WithMessage(ValidationMessages.NotEmptyFormat(nameof(IUserUuidCarrier.UserUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessages.InvalidFormat(nameof(IUserUuidCarrier.UserUuid)));
        }
    }
}
