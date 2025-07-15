using Domain.Entities.Base.Concrete;

namespace Domain.Entities.LogEntities.AuthorizationModulLogEntities.RolesLog
{
    public class RoleLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Original entity properties
        public Guid RoleUuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEditable { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsAssignedToUniversity { get; set; }
        public Guid? UniversityUuid { get; set; }
        public Guid? CreatorAdminUuid { get; set; }
        public Guid? CreatorStaffUuid { get; set; }
        public Guid? CreatorStudentUuid { get; set; }

        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }


}
