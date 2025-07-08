using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Buisness.Concrete.Dto;
using Buisness.Converters.JsonConverters;

namespace Buisness.DTOs.UniversityDtos
{
    public class CreateUniversityDto : DtoBase     
    {
        public string UniversityName { get; set; } = string.Empty;
        public string UniversityCode { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public int UniversityTypeId { get; set; } 
        public int EstablishedYear { get; set; }
        public string WebsiteUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
