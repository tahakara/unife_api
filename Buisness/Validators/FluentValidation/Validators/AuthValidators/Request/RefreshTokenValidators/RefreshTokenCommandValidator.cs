using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.RefreshTokenValidators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        private const int MaxAccessTokenLength = 3000;
        public RefreshTokenCommandValidator()
        {
            Include(new NullOrValidAccessTokenCarrierValidator<RefreshTokenCommand>());

            RuleFor(x => x.RefreshToken)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(RefreshTokenCommand.RefreshToken)))

                .MinimumLength(1)
                    .WithMessage(ValidationMessage.MinLengthFormat(nameof(RefreshTokenCommand.RefreshToken), 1))
                .MaximumLength(500)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(RefreshTokenCommand.RefreshToken), 500));
        }
        
    }
}
