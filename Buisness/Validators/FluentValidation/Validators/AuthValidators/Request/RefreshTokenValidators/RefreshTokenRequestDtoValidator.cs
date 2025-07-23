using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.RefreshTokenValidators
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        private const int MaxAccessTokenLength = 3000;
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token cannot be empty.")
                .NotNull().WithMessage("Refresh token cannot be null.")
                .Length(1, 500).WithMessage("Refresh token must be between 1 and 500 characters long.");
            RuleFor(x => x.AccessToken)
                .Must(IsNullOrValidAccessToken).WithMessage("The provided access token is invalid.");
        }

        private bool IsNullOrValidAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return true;

            var trimmedToken = token.Trim();

            return ValidationHelper.BeAValidJWTBeararToken(token);
        }
    }
}
