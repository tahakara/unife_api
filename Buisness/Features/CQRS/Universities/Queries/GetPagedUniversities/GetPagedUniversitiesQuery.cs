using Buisness.DTOs.Common;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base.Generic.Request.Query;

namespace Buisness.Features.CQRS.Universities.Queries.GetPagedUniversities
{
    public class GetPagedUniversitiesQuery : IQuery<PagedResponse<SelectUniversityDto>>
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "CreatedAt";
        public string OrderDirection { get; set; } = "asc";

        public GetPagedUniversitiesQuery()
        {
        }

        public GetPagedUniversitiesQuery(int currentPage, int pageSize)
        {
            CurrentPage = currentPage <= 0 ? 1 : currentPage;
            PageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;
            OrderBy = "CreatedAt";
            OrderDirection = "asc";
        }

        public GetPagedUniversitiesQuery(int currentPage, int pageSize, string orderBy, string orderDirection)
        {
            CurrentPage = currentPage <= 0 ? 1 : currentPage;
            PageSize = pageSize <= 0 ? 10 : pageSize > 100 ? 100 : pageSize;
            OrderBy = !string.IsNullOrWhiteSpace(orderBy) ? orderBy : "CreatedAt";
            OrderDirection = !string.IsNullOrWhiteSpace(orderDirection) ? orderDirection.ToLower() : "asc";
        }

        public void ValidateAndSetDefaults()
        {
            CurrentPage = CurrentPage <= 0 ? 1 : CurrentPage;
            PageSize = PageSize <= 0 ? 10 : PageSize > 100 ? 100 : PageSize;
            
            // Allowed order by fields
            var allowedOrderByFields = new[] 
            { 
                "UniversityName", "UniversityCode", "EstablishedYear", 
                "RegionId", "UniversityTypeId", "CreatedAt", "UpdatedAt" 
            };
            
            if (string.IsNullOrWhiteSpace(OrderBy) || !allowedOrderByFields.Contains(OrderBy, StringComparer.OrdinalIgnoreCase))
            {
                OrderBy = "CreatedAt";
            }
            
            if (string.IsNullOrWhiteSpace(OrderDirection) || 
                (OrderDirection.ToLower() != "asc" && OrderDirection.ToLower() != "desc"))
            {
                OrderDirection = "asc";
            }
        }
    }
}