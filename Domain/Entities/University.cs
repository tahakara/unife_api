using Domain.Entities.AuthorizationEntities;
using Domain.Entities.AuthorizationEntities.Permissions;
using Domain.Entities.AuthorizationEntities.Roles;
using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base.Concrete
{

    public class University : BaseEntity
    {
        public Guid UniversityUuid { get; set; } = Guid.NewGuid();
        public string UniversityName { get; set; } = string.Empty;
        public string? UniversityCode { get; set; }
        public int? RegionId { get; set; }
        public int? UniversityTypeId { get; set; }
        public Guid? AddressUuid { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? EstablishedYear { get; set; }
        public string? WebsiteUrl { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual Region? Region { get; set; }
        public virtual UniversityType? UniversityType { get; set; }
        public virtual ICollection<UniversityCommunication> UniversityCommunications { get; set; } = new List<UniversityCommunication>();
        public virtual ICollection<UniversityAddress> UniversityAddresses { get; set; } = new List<UniversityAddress>();
        public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
        public virtual ICollection<UniversityFacultyDepartment> UniversityFacultyDepartments { get; set; } = new List<UniversityFacultyDepartment>();
        public virtual ICollection<Academician> Academicians { get; set; } = new List<Academician>();
        public virtual Rector? Rector { get; set; }
        public virtual UniversityAddress? MainAddress { get; set; }

        public virtual Admin? Admin { get; set; }
        public virtual ICollection<Staff>? StaffMembers { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
        public virtual ICollection<Role>? Roles { get; set; } = new List<Role>();
        public virtual ICollection<Permission>? Permissions { get; set; } = new List<Permission>();
    }
}
