using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IUserTypeIdCarrier"/>.
    /// Ensures the user type ID is not null, not empty, greater than zero, and a valid byte value.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IUserTypeIdCarrier"/>.</typeparam>
    public class UserTypeIdCarrierValidator<T> : AbstractValidator<T>
        where T : IUserTypeIdCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTypeIdCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IUserTypeIdCarrier.UserTypeId"/> property.
        /// </summary>
        public UserTypeIdCarrierValidator()
        {
            RuleFor(x => x.UserTypeId)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IUserTypeIdCarrier.UserTypeId)))
                
                .GreaterThan((byte)0)
                    .WithMessage(ValidationMessages.GreaterThanZeroFormat(nameof(IUserTypeIdCarrier.UserTypeId)))

                .Must(ValidationHelper.BeAValidByte)
                    .WithMessage(ValidationMessages.InvalidByte(nameof(IUserTypeIdCarrier.UserTypeId)));
        }
    }
}
