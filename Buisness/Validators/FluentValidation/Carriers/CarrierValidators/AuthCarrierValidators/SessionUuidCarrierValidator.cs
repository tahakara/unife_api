using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                .NotEmpty()
                    .WithMessage(ValidationMessages.NotEmptyFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage(ValidationMessages.InvalidFormat(nameof(ISessionUuidCarrier.SessionUuid)));
        }
    }
}
