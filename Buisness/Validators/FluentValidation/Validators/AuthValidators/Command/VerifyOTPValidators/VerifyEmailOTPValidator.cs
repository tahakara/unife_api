using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail;
using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Command.VerifyOTPValidators
{
    public class VerifyEmailOTPValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailOTPValidator() 
        {
            Include(new AccessTokenCarrierValidator<VerifyEmailCommand>());
            
            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFormat(nameof(VerifyEmailCommand.OtpCode)))
                .Length(6).WithMessage(ValidationMessages.LengthBetweenFormat(nameof(VerifyEmailCommand.OtpCode), 6, 16));
        }
    }
    public class SendEmailVerificationOTPValidator : AbstractValidator<SendEmailVerificationOTPCommand>
    {
        public SendEmailVerificationOTPValidator() 
        {
            Include(new AccessTokenCarrierValidator<SendEmailVerificationOTPCommand>());
        }
    }
}
