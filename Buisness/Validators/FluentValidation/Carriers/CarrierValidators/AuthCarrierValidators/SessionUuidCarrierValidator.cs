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
                    //.WithMessage(ValidationMessages.RequiredFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                    .WithMessage("{PropertyName}")
                .NotEmpty()
                    .WithMessage(ValidationMessages.NotEmptyFormat(nameof(ISessionUuidCarrier.SessionUuid)))
                    .WithMessage("{PropertyName}")

                .Must(ValidationHelper.BeAValidUuid)
                    .WithMessage("{PropertyName}");

                //.WithMessage(ValidationMessages.InvalidFormat(nameof(ISessionUuidCarrier.SessionUuid)));
        }
    }
}
