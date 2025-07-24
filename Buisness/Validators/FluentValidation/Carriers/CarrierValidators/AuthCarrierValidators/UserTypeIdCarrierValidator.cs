using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class UserTypeIdCarrierValidator<T> : AbstractValidator<T>
        where T : IUserTypeIdCarrier
    {
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
