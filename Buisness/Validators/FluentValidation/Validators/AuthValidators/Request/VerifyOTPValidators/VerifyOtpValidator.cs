using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.DTOs.ModelBinderHelper;
using Buisness.Validators.FluentValidation.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.VerifyOTPValidators
{
    public class VerifyOTPRequestDtoValidator : AbstractValidator<VerifyOTPRequestDto>
    {
        public VerifyOTPRequestDtoValidator()
        {
            RuleFor(x => x.UserTypeId)
                .NotNull().WithMessage("UserTypeId cannot be null.")
                .NotEmpty().WithMessage("UserTypeId cannot be empty.")
                .Must(ValidationHelper.BeAValidByte).WithMessage("UserTypeId must be between 1 and 255.");

            RuleFor(x => x.UserUuid)
                .NotNull().WithMessage("UserUuid cannot be null.")
                .NotEmpty().WithMessage("UserUuid cannot be empty.")
                .Must(uuid => ValidationHelper.BeAValidUuid(uuid.ToString())).WithMessage("UserUuid must be a valid UUID.");

            RuleFor(x => x.SessionUuid)
                .NotNull().WithMessage("SessionUuid cannot be null.")
                .NotEmpty().WithMessage("SessionUuid cannot be empty.")
                .Must(uuid => ValidationHelper.BeAValidUuid(uuid.ToString())).WithMessage("SessionUuid must be a valid UUID.");

            RuleFor(x => x.OtpTypeId)
                .NotNull().WithMessage("OtpTypeId cannot be null.")
                .NotEmpty().WithMessage("OtpTypeId cannot be empty.")
                .Must(ValidationHelper.BeAValidByte).WithMessage("OtpTypeId must be between 1 and 255.");

            RuleFor(x => x.OtpCode)
                .NotNull().WithMessage("OtpCode cannot be null.")
                .NotEmpty().WithMessage("OtpCode cannot be empty.")
                .Must(ValidationHelper.BeA6DigitValidOtpCode).WithMessage("OtpCode must be a valid OTP code.");
        }
    }
}
