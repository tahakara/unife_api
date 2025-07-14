using Domain.Entities.AuthorizationEntities.Permissions;
using Domain.Entities.AuthorizationEntities.Roles;
using Domain.Entities.AuthorizationEntities.SecurityEvents;
using Domain.Entities.AuthorizationEntities.Suspensions;
using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities
{
    public class Staff : BaseEntity
    {
        public Guid StaffUuid { get; set; }
        public Guid? UniversityUuid { get; set; } // Guid? olarak değiştirildi
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
        public bool IsDeleted { get; set; } = false; // Eksik property eklendi

        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual University? University { get; set; }

        public virtual ICollection<StaffSuspension>? StaffSuspensions { get; set; }
        public virtual ICollection<SecurityEvent>? SecurityEvents { get; set; }

        // Creator navigation properties
        public virtual ICollection<Role>? CreatedRoles { get; set; }
        public virtual ICollection<Permission>? CreatedPermissions { get; set; }

        // Suspension related navigation properties
        public virtual ICollection<StaffSuspension>? SuspendedStaffSuspensions { get; set; }
        public virtual ICollection<StudentSuspension>? SuspendedStudentSuspensions { get; set; }

        // Role and Permission related navigation properties
        //public virtual ICollection<AdminRole>? AdminRoles { get; set; }
        public virtual ICollection<StaffRole>? StaffRoles { get; set; }
        public virtual ICollection<StaffPermission>? StaffPermissions { get; set; }
    }


}


