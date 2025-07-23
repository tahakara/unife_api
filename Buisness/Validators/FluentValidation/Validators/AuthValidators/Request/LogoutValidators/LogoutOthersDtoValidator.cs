using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.LogoutValidators
{
    public class LogoutOthersRequestDtoValidator : AbstractValidator<LogoutOthersRequestDto>
    {
        public LogoutOthersRequestDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().NotNull()
                .WithMessage("Access token is required.")
                .Must(ValidationHelper.BeAValidJWTBeararToken).WithMessage("Access token must be a valid JWT token.");
        }
    }
}
