using Buisness.DTOs.AuthDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators
{
    public class AccessTokenDtoValidator : AbstractValidator<AccessTokenDto>
    {
        public AccessTokenDtoValidator()
        {
            RuleFor(accessToken => accessToken.AccessToken)
                .NotNull().WithMessage("Access token cannot be null.")
                .NotEmpty().WithMessage("Access token cannot be empty.")
                .Must(BeAValidToken).WithMessage("Access token is not valid.");
        }

        private bool BeAValidToken(string token)
        {
            // Here you can implement your logic to validate the access token.
            // For example, check if it matches a specific format or is present in a database.
            // This is a placeholder implementation.
            return !string.IsNullOrWhiteSpace(token) && token.Length > 10; // Example condition
        }
    }
}
