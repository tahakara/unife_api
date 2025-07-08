using Domain.Entities.Base.Abstract;
using Domain.Entities.Base.Concrete;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    public class UniversityAdmin : BaseEntity
    {
        public Guid UniversityAdminUuid { get; set; } = Guid.NewGuid(); 
        public string AdminEmail { get; set; } = string.Empty; 
        public string AdminSupportMail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty; 
        public string? MiddleName { get; set; }
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); 
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); 
        public bool IsActive { get; set; } = true; 
        public bool IsVerified { get; set; } = false; 
        public bool IsDeleted { get; set; } = false; 
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime? PhoneVerifiedAt { get; set; }
        public DateTime? PasswordChangedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int FailedLoginAttempts { get; set; } = 0; 
        public DateTime? LockedUntil { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // Navigation Properties
        public virtual ICollection<UniversityAdminClaim> AdminClaims { get; set; } = new List<UniversityAdminClaim>(); 
        public virtual ICollection<UniversityAdminRole> AdminRoles { get; set; } = new List<UniversityAdminRole>(); 
        public virtual ICollection<UniversityAdminSession> AdminSessions { get; set; } = new List<UniversityAdminSession>();
        public virtual ICollection<UniversityAdminPermission> AdminPermissions { get; set; } = new List<UniversityAdminPermission>();
    }
    public class UniversityAdminRole : BaseEntity
    {
        public Guid AdminRoleUuid { get; set; } = Guid.NewGuid(); 
        public Guid UniversityAdminUuid { get; set; }
        public Guid RoleUuid { get; set; }
        public bool IsActive { get; set; } = true; 
        public DateTime? ExpiresAt { get; set; }
        public string? GrantedBy { get; set; }
        public string? Notes { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // Navigation Properties
        public virtual UniversityAdmin UniversityAdmin { get; set; } = null!; 
        public virtual AdminRole Role { get; set; } = null!;
    }
    public class UniversityAdminClaim : BaseEntity
    {
        public Guid AdminClaimUuid { get; set; } = Guid.NewGuid(); 
        public Guid UniversityAdminUuid { get; set; }
        public Guid ClaimUuid { get; set; }
        public string ClaimValue { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true; 
        public DateTime? ExpiresAt { get; set; }
        public string? GrantedBy { get; set; }
        public string? Notes { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // Navigation Properties
        public virtual UniversityAdmin UniversityAdmin { get; set; } = null!; 
        public virtual AdminClaim Claim { get; set; } = null!;
    }
    public class UniversityAdminPermission : BaseEntity
    {
        public Guid AdminPermissionUuid { get; set; } = Guid.NewGuid();
        public Guid UniversityAdminUuid { get; set; }
        public Guid PermissionUuid { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Scope { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? ExpiresAt { get; set; }
        public string? GrantedBy { get; set; }
        public string? Notes { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual UniversityAdmin UniversityAdmin { get; set; } = null!;
        public virtual AdminPermission Permission { get; set; } = null!;
    }
    public class AdminRole : BaseEntity
    {
        public Guid RoleUuid { get; set; } = Guid.NewGuid(); 
        public string RoleName { get; set; } = string.Empty; 
        public string RoleDisplayName { get; set; } = string.Empty; 
        public string? RoleDescription { get; set; }
        public bool IsActive { get; set; } = true; 
        public bool IsSystemRole { get; set; } = false; 
        public int Priority { get; set; } = 0; 
        public string? Category { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // Navigation Properties
        public virtual ICollection<UniversityAdminRole> AdminRoles { get; set; } = new List<UniversityAdminRole>(); 
        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
    public class AdminClaim : BaseEntity
    {
        public Guid ClaimUuid { get; set; } = Guid.NewGuid(); 
        public string ClaimType { get; set; } = string.Empty; 
        public string ClaimName { get; set; } = string.Empty; 
        public string ClaimDisplayName { get; set; } = string.Empty; 
        public string? ClaimDescription { get; set; }
        public bool IsActive { get; set; } = true; 
        public bool IsSystemClaim { get; set; } = false; 
        public string? Category { get; set; }
        public string? Resource { get; set; }
        public string? Action { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // Navigation Properties
        public virtual ICollection<UniversityAdminClaim> AdminClaims { get; set; } = new List<UniversityAdminClaim>(); 
        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
    public class AdminPermission : BaseEntity
    {
        public Guid PermissionUuid { get; set; } = Guid.NewGuid();
        public string PermissionName { get; set; } = string.Empty;
        public string PermissionDisplayName { get; set; } = string.Empty;
        public string? PermissionDescription { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsSystemPermission { get; set; } = false;
        public string? Category { get; set; }
        public int Priority { get; set; } = 0;
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<UniversityAdminPermission> AdminPermissions { get; set; } = new List<UniversityAdminPermission>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
    public class RoleClaim : BaseEntity
    {
        public Guid RoleClaimUuid { get; set; } = Guid.NewGuid(); 
        public Guid RoleUuid { get; set; }
        public Guid ClaimUuid { get; set; }
        public string ClaimValue { get; set; } = string.Empty; 
        public bool IsActive { get; set; } = true; 
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // Navigation Properties
        public virtual AdminRole Role { get; set; } = null!; 
        public virtual AdminClaim Claim { get; set; } = null!;
    }
    public class RolePermission : BaseEntity
    {
        public Guid RolePermissionUuid { get; set; } = Guid.NewGuid();
        public Guid RoleUuid { get; set; }
        public Guid PermissionUuid { get; set; }
        public bool IsActive { get; set; } = true;
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual AdminRole Role { get; set; } = null!;
        public virtual AdminPermission Permission { get; set; } = null!;
    }
    public class UniversityAdminSession : BaseEntity
    {
        public Guid SessionUuid { get; set; } = Guid.NewGuid(); 
        public Guid UniversityAdminUuid { get; set; }
        public string SessionToken { get; set; } = string.Empty; 
        public string IpAddress { get; set; } = string.Empty; 
        public string? UserAgent { get; set; }
        public bool IsActive { get; set; } = true; 
        public DateTime? LastActivityAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? DeviceInfo { get; set; }
        public string? Location { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual UniversityAdmin UniversityAdmin { get; set; } = null!;
    }
    public class UniversityAdminSecurityEvent : BaseEntity
    {
        public Guid SecurityEventUuid { get; set; } = Guid.NewGuid();
        public Guid UniversityAdminUuid { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventDescription { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
        public bool IsSuccessful { get; set; }
        public string? AdditionalData { get; set; }
        public override DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public override DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual UniversityAdmin UniversityAdmin { get; set; } = null!;
    }
    public class UniversityAdminLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid UniversityAdminUuid { get; set; }
        public string? AdminEmail { get; set; }
        public string? AdminSupportMail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime? PhoneVerifiedAt { get; set; }
        public DateTime? PasswordChangedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime OriginalCreatedAt { get; set; }
        public DateTime OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Navigation Properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
    public class UniversityAdminClaimLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid AdminClaimUuid { get; set; }
        public Guid UniversityAdminUuid { get; set; }
        public Guid ClaimUuid { get; set; }
        public string? ClaimValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? GrantedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime OriginalCreatedAt { get; set; }
        public DateTime OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow; 
        public override string? LoggedBy { get; set; } = "System"; 

        // Navigation Properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
    public class UniversityAdminRoleLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid AdminRoleUuid { get; set; }
        public Guid UniversityAdminUuid { get; set; }
        public Guid RoleUuid { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? GrantedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime OriginalCreatedAt { get; set; }
        public DateTime OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow; 
        public override string? LoggedBy { get; set; } = "System"; 

        // Navigation Properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
    public class UniversityAdminPermissionLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid AdminPermissionUuid { get; set; }
        public Guid UniversityAdminUuid { get; set; }
        public Guid PermissionUuid { get; set; }
        public string? Resource { get; set; }
        public string? Action { get; set; }
        public string? Scope { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? GrantedBy { get; set; }
        public string? Notes { get; set; }
        public DateTime OriginalCreatedAt { get; set; }
        public DateTime OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Navigation Properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
    public class UniversityAdminSecurityEventLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid SecurityEventUuid { get; set; }
        public Guid UniversityAdminUuid { get; set; }
        public string? EventType { get; set; }
        public string? EventDescription { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsSuccessful { get; set; }
        public string? AdditionalData { get; set; }
        public DateTime OriginalCreatedAt { get; set; }
        public DateTime OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Navigation Properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
}