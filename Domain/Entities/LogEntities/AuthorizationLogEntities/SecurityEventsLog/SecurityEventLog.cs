using Domain.Entities.Base.Concrete;

namespace Domain.Entities.LogEntities.AuthorizationLogEntities.SecurityEventsLog
{
    public class SecurityEventLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Original entity properties
        public Guid SecurityEventUuid { get; set; }
        public Guid? EventTypeUuid { get; set; }
        public Guid? EventedByAdminUuid { get; set; }
        public Guid? EventedByStaffUuid { get; set; }
        public Guid? EventedByStudentUuid { get; set; }
        public string Description { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
        public string? AdditionalData { get; set; }
        public DateTime EventTime { get; set; }

        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }


}
