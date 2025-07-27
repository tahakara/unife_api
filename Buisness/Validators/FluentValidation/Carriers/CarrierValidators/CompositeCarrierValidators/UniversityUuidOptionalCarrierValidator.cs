using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IUniversityOptionalCarrier"/>.
    /// Ensures the university UUID is either null or a valid UUID, and not empty if provided.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IUniversityOptionalCarrier"/>.</typeparam>
    public class UniversityUuidOptionalCarrierValidator<T> : AbstractValidator<T>
        where T : IUniversityOptionalCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniversityUuidOptionalCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IUniversityOptionalCarrier.UniversityUuid"/> property.
        /// </summary>
        public UniversityUuidOptionalCarrierValidator()
        {
            RuleFor(x => x.UniversityUuid)
                .Must(uuid => uuid == null || ValidationHelper.BeAValidUuid(uuid))
                    .WithMessage(ValidationMessages.InvalidFormat(nameof(IUniversityOptionalCarrier.UniversityUuid)))

                .NotEmpty()
                    .When(x => x.UniversityUuid != null)
                    .WithMessage(ValidationMessages.NotEmptyFormat(nameof(IUniversityOptionalCarrier.UniversityUuid)));
        }
    }
}
