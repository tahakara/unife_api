using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Validators.FluentValidation.Validators.Common;
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
                .Must(ValidationHelper.BeAValidJWTToken).WithMessage("Access token must be a valid JWT token.");
        }
    }
}
