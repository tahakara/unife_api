using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities.UniversityModul
{

    public class UniversityAddress : BaseEntity
    {
        public Guid AddressUuid { get; set; } = Guid.NewGuid();
        public Guid UniversityUuid { get; set; }
        public int AddressTypeId { get; set; }
        public string? AddressTitle { get; set; }
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateProvince { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int Priority { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual University University { get; set; } = null!;
        public virtual AddressType AddressType { get; set; } = null!;
        public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
        public virtual ICollection<University> UniversitiesAsMainAddress { get; set; } = new List<University>();
    }
}
