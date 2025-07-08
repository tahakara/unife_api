using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Validators.Common.Request
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        public PaginationRequestValidator()
        {
            RuleFor(x => x.CurrentPage)
                .GreaterThan(0).WithMessage("Current page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 10000).WithMessage("Page size must be between 1 and 10000.");

            RuleFor(x => x.TotalCount)
                .GreaterThanOrEqualTo(0).WithMessage("Total count cannot be negative.");
        }
    }

    public class PaginationRequest
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
    }
}
