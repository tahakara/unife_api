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
    /// <summary>
    /// Validator for types implementing <see cref="IUserUuidCarrier"/>.
    /// Ensures the user UUID is not null, not empty, and is a valid UUID format.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IUserUuidCarrier"/>.</typeparam>
    public class UserUuidCarrierValidator<T> : AbstractValidator<T>
        where T : IUserUuidCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUuidCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IUserUuidCarrier.UserUuid"/> property.
        /// </summary>
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
