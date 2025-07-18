using Buisness.DTOs.AuthDtos.RefreshDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.RefreshTokenValidators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        private const int MaxAccessTokenLength = 3000;
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token cannot be empty.")
                .NotNull()
                .WithMessage("Refresh token cannot be null.")
                .Length(1, 500)
                .WithMessage("Refresh token must be between 1 and 500 characters long.");
            RuleFor(x => x.AccessToken)
                .Must(IsNullOrValidAccessToken)
                .WithMessage("The provided access token is invalid."); // 🔒 Secure generic message
        }

        private bool IsNullOrValidAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return true;

            var trimmedToken = token.Trim();

            return IsValidLength(trimmedToken)
                && HasMinimumDotCount(trimmedToken)
                && MatchesJwtPattern(trimmedToken);
        }

        private bool IsValidLength(string token)
            => token.Length <= MaxAccessTokenLength;

        private bool HasMinimumDotCount(string token)
            => token.Count(c => c == '.') >= 2;

        private bool MatchesJwtPattern(string token)
        {
            var jwtRegex = @"^[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+\.[A-Za-z0-9\-_]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(token, jwtRegex);
        }
    }
}
