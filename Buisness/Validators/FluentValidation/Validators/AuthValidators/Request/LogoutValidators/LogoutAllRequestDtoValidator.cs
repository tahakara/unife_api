using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.LogoutValidators
{
    public class LogoutAllRequestDtoValidator : AbstractValidator<LogoutAllRequestDto>
    {
        public LogoutAllRequestDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().NotNull()
                .WithMessage("Access token is required.")
                .Matches(@"^[a-zA-Z0-9\-_]+$")
                .WithMessage("Access token contains invalid characters.");

        }
    }
}
