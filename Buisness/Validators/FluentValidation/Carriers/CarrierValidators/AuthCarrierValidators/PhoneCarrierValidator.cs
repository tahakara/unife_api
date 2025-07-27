using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IPhoneCarrier"/>.
    /// Ensures the phone country code and phone number are not null, not empty, and are in valid formats.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IPhoneCarrier"/>.</typeparam>
    public class PhoneCarrierValidator<T> : AbstractValidator<T>
        where T : IPhoneCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IPhoneCarrier.PhoneCountryCode"/> and <see cref="IPhoneCarrier.PhoneNumber"/> properties.
        /// </summary>
        public PhoneCarrierValidator()
        {
            RuleFor(x => x.PhoneCountryCode)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IPhoneCarrier.PhoneCountryCode)))

                .Must(ValidationHelper.BeAValidCountryCode)
                    .WithMessage(ValidationMessages.PhoneCountryCodeFormat(nameof(IPhoneCarrier.PhoneCountryCode)));
            
            RuleFor(x => x.PhoneNumber)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IPhoneCarrier.PhoneNumber)))

                .Must(ValidationHelper.BeAValidPhoneNumber)
                    .WithMessage(ValidationMessages.PhoneNumberFormat(nameof(IPhoneCarrier.PhoneNumber)));
        }
    }
}
