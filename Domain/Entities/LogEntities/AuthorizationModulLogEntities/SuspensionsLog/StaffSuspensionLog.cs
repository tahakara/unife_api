using Domain.Entities.Base.Concrete;

namespace Domain.Entities.LogEntities.AuthorizationModulLogEntities.SuspensionsLog
{
    public class StaffSuspensionLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Original entity properties
        public Guid StaffSuspensionUuid { get; set; }
        public Guid StaffUuid { get; set; }
        public Guid? SuspendedByAdminUuid { get; set; }
        public Guid? SuspendedByStaffUuid { get; set; }
        public DateTime SuspensionStartDate { get; set; }
        public DateTime SuspensionEndDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }


}
