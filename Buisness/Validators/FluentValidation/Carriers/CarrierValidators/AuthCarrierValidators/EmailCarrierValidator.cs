using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IEmailCarrier.Email)))
                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IEmailCarrier.Email), 100))
                .EmailAddress()
                    .WithMessage(ValidationMessages.EmailFormat(nameof(IEmailCarrier.Email)))
                .Must(ValidationHelper.BeAValidEmail)
                    .WithMessage(ValidationMessages.NotAccepedEmailFormat(nameof(IEmailCarrier.Email)));
        }
    }
}
