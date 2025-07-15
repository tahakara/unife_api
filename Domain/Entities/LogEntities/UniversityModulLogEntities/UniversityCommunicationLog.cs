using Core.Entities.Base.Concrete;

namespace Domain.Entities.LogEntities.UniversityModulLogEntities
{
    public class UniversityCommunicationLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid? CommunicationUuid { get; set; }
        public Guid? UniversityUuid { get; set; }
        public int? TypeId { get; set; }
        public string? CommunicationValue { get; set; }
        public int? Priority { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }

        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;

    }
}
