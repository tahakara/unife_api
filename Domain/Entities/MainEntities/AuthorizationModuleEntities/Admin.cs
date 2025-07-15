using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Permissions;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Roles;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Suspensions;
using Domain.Entities.MainEntities.UniversityModul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities
{
    public class Admin : BaseEntity
    {
        public Guid AdminUuid { get; set; }
        public Guid? UniversityUuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool IsPhoneNumberVerified { get; set; } = false;


        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string? ProfileImageUrl { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsSystemPermission { get; set; } = false;

        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual University? University { get; set; }
        public virtual ICollection<SecurityEvent> SecurityEvents { get; set; } = new List<SecurityEvent>();

        // Creator navigation properties
        public virtual ICollection<Role>? CreatedRoles { get; set; } = new List<Role>();
        public virtual ICollection<Permission>? CreatedPermissions { get; set; } = new List<Permission>();

        // Junction table navigation properties
        public virtual ICollection<AdminRole>? AdminRoles { get; set; } = new List<AdminRole>();
        public virtual ICollection<AdminPermission> AdminPermissions { get; set; } = new List<AdminPermission>();

        // Suspension related navigation properties
        public virtual ICollection<StaffSuspension>? SuspendedStaffSuspensions { get; set; } = new List<StaffSuspension>();
        public virtual ICollection<StudentSuspension>? SuspendedStudentSuspensions { get; set; } = new List<StudentSuspension>();

    }


}


