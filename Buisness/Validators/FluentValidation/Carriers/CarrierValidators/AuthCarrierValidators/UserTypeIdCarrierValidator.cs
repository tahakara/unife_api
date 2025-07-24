using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
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
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IUserTypeIdCarrier.UserTypeId)))
                
                .GreaterThan((byte)0)
                    .WithMessage(ValidationMessage.GreaterThanZeroFormat(nameof(IUserTypeIdCarrier.UserTypeId)))

                .Must(ValidationHelper.BeAValidByte)
                    .WithMessage(ValidationMessage.InvalidByte(nameof(IUserTypeIdCarrier.UserTypeId)));
        }
    }
}
