using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.LogoutValidators
{
    public class LogoutRequestDtoValidator : AbstractValidator<LogoutRequestDto>
    {
        public LogoutRequestDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().NotNull()
                .WithMessage("Access token is required.")
                .Matches(@"^[a-zA-Z0-9\-_]+$")
                .WithMessage("Access token contains invalid characters.");
        }
    }
}
