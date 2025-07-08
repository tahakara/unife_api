using Buisness.Concrete.Dto;

namespace Buisness.DTOs.Common
{
    public class PagedRequest<T> : DtoBase
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
