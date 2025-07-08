using Buisness.Concrete.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.UniversityDtos
{
    public class SelectUniversityDto: DtoBase
    {
        public Guid UniversityUuid { get; set; }
        public string UniversityName { get; set; } = string.Empty;
        public string? UniversityCode { get; set; }
        public int? RegionId { get; set; }
        //public string? RegionName { get; set; }  // Navigation property'den
        public int? UniversityTypeId { get; set; }
        //public string? UniversityTypeName { get; set; }  // Navigation property'den
        public bool IsActive { get; set; }
        public int? EstablishedYear { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
