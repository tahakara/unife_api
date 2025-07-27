using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base.Generic.Request.Query;
using Buisness.Features.CQRS.Base.Generic.Response;

namespace Buisness.Features.CQRS.Universities.Queries.GetUniversityById
{
    public class GetUniversityByIdQuery : IQuery<BaseResponse<SelectUniversityDto>>
    {
        public Guid UniversityId { get; set; }

        public GetUniversityByIdQuery(Guid universityId)
        {
            UniversityId = universityId;
        }
    }
}