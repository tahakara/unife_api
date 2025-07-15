using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.LogEntities.UniversityModulLogEntities
{

    public class RectorLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid? RectorUuid { get; set; }
        public Guid? UniversityUuid { get; set; }
        public Guid? AcademicianUuid { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; }
        public override string? LoggedBy { get; set; }

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
}
