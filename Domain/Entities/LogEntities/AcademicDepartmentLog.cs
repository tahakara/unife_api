using Domain.Entities.Base.Concrete;

namespace Domain.Entities.LogEntities
{
    public class AcademicDepartmentLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public int? DepartmentTypeId { get; set; }
        public string? DepartmentName { get; set; }
        public string? DepartmentCode { get; set; }
        public string? DepartmentDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }

        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
}
