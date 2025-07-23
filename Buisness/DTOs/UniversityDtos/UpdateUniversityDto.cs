using Buisness.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Buisness.DTOs.UniversityDtos
{
    public class UpdateUniversityDto : DtoBase
    {
        public Guid UniversityUuid { get; set; }
        public string UniversityName { get; set; } = string.Empty;
        public string UniversityCode { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public int UniversityTypeId { get; set; }
        public int EstablishedYear { get; set; }
        public string WebsiteUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
