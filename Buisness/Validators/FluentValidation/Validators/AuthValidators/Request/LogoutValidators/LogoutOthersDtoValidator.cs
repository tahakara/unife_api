using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
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
                .Matches(@"^[a-zA-Z0-9\-_]+$")
                .WithMessage("Access token contains invalid characters.");
        }
    }
}
