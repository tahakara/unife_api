using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.UniversityModul;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities
{

    public class Region : BaseEntity
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public string RegionCodeAlpha2 { get; set; } = string.Empty;
        public string RegionCodeAlpha3 { get; set; } = string.Empty;
        public string RegionCodeNumeric { get; set; } = string.Empty;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<University> Universities { get; set; } = new List<University>();
    }
}
