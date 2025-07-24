using Buisness.DTOs.UniversityDtos;
using Buisness.Validators.FluentValidation.Common;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Validators.University.Request
{
    public class CreateUniversityDtoValidator : AbstractValidator<CreateUniversityDto>
    {
        public CreateUniversityDtoValidator()
        {
            RuleFor(x => x.UniversityName)
                .NotNull().WithMessage("University name is required.")
                .NotEmpty().WithMessage("University name is required.")
                .Length(1, 250).WithMessage("University name must be between 1 and 250 characters.")
                .Must(ValidationHelper.BeAValidUniversityName).WithMessage("University name can only contain letters, spaces, hyphens and dots.")
                .Must(ValidationHelper.BeFullUppercase).WithMessage("University name must be only uppercase.");

            RuleFor(x => x.UniversityCode)
                .NotNull().WithMessage("University code is required.")
                .NotEmpty().WithMessage("University code cannot be empty.")
                .Length(1, 50).WithMessage("University code cannot exceed 50 characters.")
                .Must(ValidationHelper.BeAValidUniversityCode).WithMessage("University code can only contain uppercase letters, numbers and hyphens.")
                .When(x => !string.IsNullOrEmpty(x.UniversityCode));

            RuleFor(x => x.RegionId)
                .NotNull().WithMessage("Region id is required.")
                .NotEmpty().WithMessage("Region id cannot be empty.")
                .NotEqual(0).WithMessage("Region id cannot be equal to 0.")
                .GreaterThan(0).WithMessage("Region id must be greater than 0.");

            RuleFor(x => x.UniversityTypeId)
                .NotNull().WithMessage("University type is required.")
                .NotEmpty().WithMessage("University type cannot be empty.")
                .NotEqual(0).WithMessage("University type is can not equal to 0.")
                .GreaterThan(0).WithMessage("University type must be greater than 0.");

            RuleFor(x => x.EstablishedYear)
                .NotNull().WithMessage("University Establishment Year is required.")
                .NotEmpty().WithMessage("University Establishment Year cannot be empty.")
                .Must(year => ValidationHelper.BeAValidYear(year)).WithMessage("Established year must be a valid year string (e.g., '2002').")
                .WithMessage($"Established year must be between 1800 and {DateTime.Now.Year}.");

            RuleFor(x => x.WebsiteUrl)
                .NotNull().WithMessage("Website URL is required.")
                .NotEmpty().WithMessage("Website URL cannot be empty.")
                .Must(ValidationHelper.BeAValidUrl).WithMessage("Please enter a valid URL.")
                .Length(1, 500).WithMessage("Website URL cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));
        }
    }
}
