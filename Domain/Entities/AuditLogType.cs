using Domain.Entities.LogEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base.Concrete
{

    public class AuditLogType : BaseEntity
    {
        public int LogTypeId { get; set; }
        public string LogTypeName { get; set; } = string.Empty;
        public string? LogTypeDescription { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<UniversityLog> UniversityLogs { get; set; } = new List<UniversityLog>();
        public virtual ICollection<FacultyLog> FacultyLogs { get; set; } = new List<FacultyLog>();
        public virtual ICollection<AcademicianLog> AcademicianLogs { get; set; } = new List<AcademicianLog>();
        public virtual ICollection<RectorLog> RectorLogs { get; set; } = new List<RectorLog>();
        public virtual ICollection<AcademicDepartmentLog> AcademicDepartmentLogs { get; set; } = new List<AcademicDepartmentLog>();
        public virtual ICollection<UniversityCommunicationLog> UniversityCommunicationLogs { get; set; } = new List<UniversityCommunicationLog>();
        public virtual ICollection<UniversityAddressLog> UniversityAddressLogs { get; set; } = new List<UniversityAddressLog>();
        public virtual ICollection<UniversityFacultyDepartmentLog> UniversityFacultyDepartmentLogs { get; set; } = new List<UniversityFacultyDepartmentLog>();
    }
}
