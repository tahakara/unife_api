using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AcademicModulEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities.UniversityModul
{

    public class Faculty : BaseEntity
    {
        public Guid FacultyUuid { get; set; } = Guid.NewGuid();
        public Guid UniversityUuid { get; set; }
        public string FacultyName { get; set; } = string.Empty;
        public string? FacultyCode { get; set; }
        public string? FacultyDescription { get; set; }
        public Guid? DeanUuid { get; set; }
        
        // Faculty'nin adresi için foreign key - One-to-Many ilişki
        public Guid? AddressUuid { get; set; }
        
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual University University { get; set; } = null!;
        public virtual Academician? Dean { get; set; }
        public virtual ICollection<UniversityFacultyDepartment> UniversityFacultyDepartments { get; set; } = new List<UniversityFacultyDepartment>();
        public virtual ICollection<Academician> Academicians { get; set; } = new List<Academician>();
        public virtual UniversityAddress? Address { get; set; }
    }
}
