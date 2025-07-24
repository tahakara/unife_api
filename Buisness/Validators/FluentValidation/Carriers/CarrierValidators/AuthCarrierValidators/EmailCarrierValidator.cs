using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class EmailCarrierValidator<T> : AbstractValidator<T>
        where T : IEmailCarrier
    {
        public EmailCarrierValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IEmailCarrier.Email)))
                .MaximumLength(100)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(IEmailCarrier.Email), 100))
                .EmailAddress()
                    .WithMessage(ValidationMessage.EmailFormat(nameof(IEmailCarrier.Email)))
                .Must(ValidationHelper.BeAValidEmail)
                    .WithMessage(ValidationMessage.NotAccepedEmailFormat(nameof(IEmailCarrier.Email)));
        }
    }
}
