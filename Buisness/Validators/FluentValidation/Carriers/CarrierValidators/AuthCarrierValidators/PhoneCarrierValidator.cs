using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class PhoneCarrierValidator<T> : AbstractValidator<T>
        where T : IPhoneCarrier
    {
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
