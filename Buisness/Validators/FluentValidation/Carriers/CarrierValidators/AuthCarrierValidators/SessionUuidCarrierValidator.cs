using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class SessionUuidCarrierValidator<T> : AbstractValidator<T>
        where T : ISessionUuidCarrier
    {
        public SessionUuidCarrierValidator()
        {
            RuleFor(x => x.SessionUuid)
                .NotNull()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                .NotEmpty()
                    .WithMessage(ValidationMessage.NotEmptyFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessage.InvalidFormat(nameof(ISessionUuidCarrier.SessionUuid)));
        }
    }
}
