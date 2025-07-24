using Buisness.DTOs.UniversityDtos;
using Buisness.Validators.FluentValidation.Common;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Validators.University.Request
{
    public class UpdateUniversityDtoValidator : AbstractValidator<UpdateUniversityDto>
    {
        public UpdateUniversityDtoValidator()
        {
            RuleFor(x => x.UniversityUuid)
                .NotNull().WithMessage("University UUID is required.")
                .NotEmpty().WithMessage("University UUID is required.");

            RuleFor(x => x.UniversityName)
                .NotNull().WithMessage("University name is required.")
                .NotEmpty().WithMessage("University name is required.")
                .Length(1, 250).WithMessage("University name must be between 1 and 250 characters.")
                .Must(ValidationHelper.BeAValidUniversityName)
                .WithMessage("University name can only contain letters, spaces, hyphens and dots.");

            RuleFor(x => x.UniversityCode)
                .NotNull().WithMessage("University code is required.")
                .NotEmpty().WithMessage("University code cannot be empty.")
                .Length(1, 50).WithMessage("University code cannot exceed 50 characters.")
                .Must(ValidationHelper.BeAValidUniversityCode)
                .WithMessage("University code can only contain uppercase letters, numbers and hyphens.")
                .When(x => !string.IsNullOrEmpty(x.UniversityCode));

            RuleFor(x => x.RegionId)
                .NotNull().WithMessage("Region ID is required.")
                .GreaterThan(0).WithMessage("Region ID must be greater than 0.");

            RuleFor(x => x.UniversityTypeId)
                .NotNull().WithMessage("University Type ID is required.")
                .GreaterThan(0).WithMessage("University Type ID must be greater than 0.");

            RuleFor(x => x.EstablishedYear)
                .NotNull().WithMessage("Established Year is required.")
                .GreaterThan(0).WithMessage("Established Year must be greater than 0.")
                .Must(year => ValidationHelper.BeAValidYear(year))
                .WithMessage($"Established year must be between 1000 and {DateTime.Now.Year}.");

            RuleFor(x => x.WebsiteUrl)
                .NotNull().WithMessage("Website URL is required.")
                .NotEmpty().WithMessage("Website URL cannot be empty.")
                .Must(ValidationHelper.BeAValidUrl)
                .WithMessage("Please enter a valid URL.")
                .Length(1, 500).WithMessage("Website URL cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive field is required for updates.");
        }
    }
}