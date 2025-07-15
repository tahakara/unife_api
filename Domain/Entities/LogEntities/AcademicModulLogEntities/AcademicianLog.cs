using Core.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.LogEntities.AcademicModulLogEntities
{

    public class AcademicianLog : LogBaseEntity
    {
        public override long LogId { get; set; }
        public override int LogTypeId { get; set; }
        public Guid? AcademicianUuid { get; set; }
        public Guid? UniversityUuid { get; set; }
        public Guid? FacultyUuid { get; set; }
        public Guid? DepartmentUuid { get; set; }
        public int? TitleId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PersonalWebsiteUrl { get; set; }
        public string? OfficeAddress { get; set; }
        public string? Bio { get; set; }
        public string? YokId { get; set; }
        public string? GoogleScholarId { get; set; }
        public string? ScopusId { get; set; }
        public string? WebOfScienceId { get; set; }
        public string? OrcidId { get; set; }
        public string? ResearchGateId { get; set; }
        public string? LinkedinUsername { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? OriginalCreatedAt { get; set; }
        public DateTime? OriginalUpdatedAt { get; set; }
        public override DateTime LoggedAt { get; set; } = DateTime.UtcNow;
        public override string? LoggedBy { get; set; } = "System"; // Default value

        // Navigation properties
        public virtual AuditLogType LogType { get; set; } = null!;
    }
}
