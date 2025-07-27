using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="ISessionUuidCarrier"/>.
    /// Ensures the session UUID is not null, not empty, and is a valid UUID.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="ISessionUuidCarrier"/>.</typeparam>
    public class SessionUuidCarrierValidator<T> : AbstractValidator<T>
        where T : ISessionUuidCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionUuidCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="ISessionUuidCarrier.SessionUuid"/> property.
        /// </summary>
        public SessionUuidCarrierValidator()
        {
            RuleFor(x => x.SessionUuid)
                .NotNull()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                
                .NotEmpty()
                    .WithMessage(ValidationMessages.NotEmptyFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessages.InvalidFormat(nameof(ISessionUuidCarrier.SessionUuid)));
        }
    }
}
