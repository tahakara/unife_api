using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
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
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IPhoneCarrier.PhoneCountryCode)))
                .Must(ValidationHelper.BeAValidCountryCode)
                    .WithMessage(ValidationMessage.PhoneCountryCodeFormat(nameof(IPhoneCarrier.PhoneCountryCode)));
            
            RuleFor(x => x.PhoneNumber)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IPhoneCarrier.PhoneNumber)))
                .Must(ValidationHelper.BeAValidPhoneNumber)
                    .WithMessage(ValidationMessage.PhoneNumberFormat(nameof(IPhoneCarrier.PhoneNumber)));
        }
    }
}
