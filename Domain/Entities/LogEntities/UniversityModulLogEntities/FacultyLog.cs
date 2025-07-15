using Core.Entities.Base.Concrete;
using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.LogEntities.UniversityModulLogEntities
{

    public class FacultyLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid? FacultyUuid { get; set; }
        public Guid? UniversityUuid { get; set; }
        public string? FacultyName { get; set; }
        public string? FacultyCode { get; set; }
        public string? FacultyDescription { get; set; }
        public Guid? DeanUuid { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System";

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
}
