using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.DTOs.ModelBinderHelper;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Command.VerifyOTPValidators
{
    public class VerifyOTPCommandValidator : AbstractValidator<VerifyOTPCommand>
    {
        public VerifyOTPCommandValidator()
        {
            Include(new UserTypeIdCarrierValidator<VerifyOTPCommand>());
            Include(new UserUuidCarrierValidator<VerifyOTPCommand>());
            Include(new SessionUuidCarrierValidator<VerifyOTPCommand>());

            RuleFor(x => x.OtpTypeId)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(VerifyOTPCommand.OtpTypeId)))

                .Must(ValidationHelper.BeAValidByte)
                    .WithMessage(ValidationMessages.InvalidByte(nameof(VerifyOTPCommand.OtpTypeId)));


            RuleFor(x => x.OtpCode)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(VerifyOTPCommand.OtpCode)))

                .Must(ValidationHelper.BeA6DigitValidOtpCode)
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(VerifyOTPCommand.OtpCode)));
        }
    }
}
