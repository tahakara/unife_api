using Core.Entities.Base.Concrete;

namespace Domain.Entities.LogEntities.AuthorizationModulLogEntities.PermissionsLog
{
    public class StudentPermissionLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Original entity properties
        public Guid StudentPermissionUuid { get; set; }
        public Guid StudentUuid { get; set; }
        public Guid PermissionUuid { get; set; }

        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }


}
