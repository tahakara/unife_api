using Buisness.DTOs.Base;

namespace Buisness.DTOs.Common
{
    /// <summary>
    /// Represents a paged request for retrieving a subset of data with pagination parameters.
    /// </summary>
    /// <typeparam name="T">The type of the data being requested.</typeparam>
    public class PagedRequest<T> : DtoBase
    {
        /// <summary>
        /// Gets or sets the current page number for the paged request.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page for the paged request.
        /// </summary>
        public int PageSize { get; set; }
    }
}
