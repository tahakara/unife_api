using Core.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Permissions;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Roles;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.Suspensions;
using Domain.Entities.MainEntities.UniversityModul;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities
{
    public class Student : BaseEntity
    {
        public Guid StudentUuid { get; set; }
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
        public bool IsDeleted { get; set; } = false;

        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual University? University { get; set; }
        public virtual ICollection<StudentSuspension>? StudentSuspensions { get; set; }
        public virtual ICollection<SecurityEvent>? SecurityEvents { get; set; }

        // Creator navigation properties
        public virtual ICollection<Role>? CreatedRoles { get; set; }
        public virtual ICollection<Permission>? CreatedPermissions { get; set; }

        // Junction table navigation properties
        public virtual ICollection<StudentRole>? StudentRoles { get; set; }
        public virtual ICollection<StudentPermission>? StudentPermissions { get; set; }

        // Eksik navigation properties eklendi
        public virtual ICollection<StudentSuspension>? SuspendedStudentSuspensions { get; set; }
    }


}


