using Buisness.DTOs.UniversityDtos;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Validators.University.Request
{
    public class DeleteUniversityDtoValidator : AbstractValidator<DeleteUniversityDto>
    {
        public DeleteUniversityDtoValidator()
        {
            RuleFor(x => x.UniversityUuid)
                .NotNull().WithMessage("University UUID cannot be null.")
                .NotEmpty().WithMessage("University UUID is required.")
                .NotEqual(Guid.Empty).WithMessage("University UUID cannot be empty GUID.");
        }
    }
}