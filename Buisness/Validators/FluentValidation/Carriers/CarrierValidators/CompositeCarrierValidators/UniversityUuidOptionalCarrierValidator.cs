using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    public class UniversityUuidOptionalCarrierValidator<T> : AbstractValidator<T>
        where T : IUniversityOptionalCarrier
    {
        public UniversityUuidOptionalCarrierValidator()
        {
            RuleFor(x => x.UniversityUuid)
                .Must(uuid => uuid == null || ValidationHelper.BeAValidUuid(uuid))
                    .WithMessage(ValidationMessage.InvalidFormat(nameof(IUniversityOptionalCarrier.UniversityUuid)))
                .NotEmpty()
                    .When(x => x.UniversityUuid != null)
                    .WithMessage(ValidationMessage.NotEmptyFormat(nameof(IUniversityOptionalCarrier.UniversityUuid)));
                
        }
    }
}
