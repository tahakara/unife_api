using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.UniversityModul;
using System;

namespace Domain.Entities.MainEntities.AcademicModulEntities
{
    public class Academician : BaseEntity
    {
        public Guid AcademicianUuid { get; set; } = Guid.NewGuid();
        public Guid? UniversityUuid { get; set; }
        public Guid? FacultyUuid { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public int? TitleId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PersonalWebsiteUrl { get; set; }
        public string? OfficeAddress { get; set; }
        public string? Bio { get; set; }
        // Academic identifiers
        public string? YokId { get; set; }
        public string? GoogleScholarId { get; set; }
        public string? ScopusId { get; set; }
        public string? WebOfScienceId { get; set; }
        public string? OrcidId { get; set; }
        public string? ResearchGateId { get; set; }
        public string? LinkedinUsername { get; set; }
        // Profile image
        public string? ProfileImageUrl { get; set; }
        // Status fields
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual University? University { get; set; }
        public virtual Faculty? Faculty { get; set; }
        public virtual UniversityFacultyDepartment? Department { get; set; }
        public virtual AcademicianTitle? Title { get; set; }
        public virtual Rector? Rector { get; set; }
        public virtual ICollection<Faculty> DeanOfFaculties { get; set; } = new List<Faculty>();
    }
}
