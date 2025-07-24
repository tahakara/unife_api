using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Command.RefreshTokenValidators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        private const int MaxAccessTokenLength = 3000;
        public RefreshTokenCommandValidator()
        {
            Include(new NullOrValidAccessTokenCarrierValidator<RefreshTokenCommand>());

            RuleFor(x => x.RefreshToken)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(RefreshTokenCommand.RefreshToken)))

                .MinimumLength(1)
                    .WithMessage(ValidationMessages.MinLengthFormat(nameof(RefreshTokenCommand.RefreshToken), 1))
                .MaximumLength(500)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(RefreshTokenCommand.RefreshToken), 500));
        }
        
    }
}
