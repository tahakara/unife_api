using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.Common.Response
{
    public class PaginationResponseValidator<T> : AbstractValidator<PaginationResponse<T>>
    {
        public PaginationResponseValidator()
        {
            RuleFor(x => x.Data)
                .NotNull().WithMessage("Data cannot be null.")
                .Must(data => data.Any()).WithMessage("Data cannot be empty.");
            RuleFor(x => x.TotalCount)
                .GreaterThanOrEqualTo(0).WithMessage("Total count must be zero or greater.");
            RuleFor(x => x.CurrentPage)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1.");
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be greater than or equal to 1.");
        }

    }

    public class PaginationResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
    }
}
