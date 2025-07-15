using Core.Entities.Base.Concrete;
using Domain.Entities.LogEntities.AcademicModulLogEntities;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.PermissionsLog;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.RolesLog;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.SecurityEventsLog;
using Domain.Entities.LogEntities.AuthorizationModulLogEntities.SuspensionsLog;
using Domain.Entities.LogEntities.UniversityModulLogEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.LogEntities
{

    public class AuditLogType : BaseEntity
    {
        public int LogTypeId { get; set; }
        public string LogTypeName { get; set; } = string.Empty;
        public string? LogTypeDescription { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<UniversityLog>? UniversityLogs { get; set; } = new List<UniversityLog>();
        public virtual ICollection<FacultyLog>? FacultyLogs { get; set; } = new List<FacultyLog>();
        public virtual ICollection<AcademicianLog>? AcademicianLogs { get; set; } = new List<AcademicianLog>();
        public virtual ICollection<RectorLog>? RectorLogs { get; set; } = new List<RectorLog>();
        public virtual ICollection<AcademicDepartmentLog>? AcademicDepartmentLogs { get; set; } = new List<AcademicDepartmentLog>();
        public virtual ICollection<UniversityCommunicationLog>? UniversityCommunicationLogs { get; set; } = new List<UniversityCommunicationLog>();
        public virtual ICollection<UniversityAddressLog>? UniversityAddressLogs { get; set; } = new List<UniversityAddressLog>();
        public virtual ICollection<UniversityFacultyDepartmentLog>? UniversityFacultyDepartmentLogs { get; set; } = new List<UniversityFacultyDepartmentLog>();

        public virtual ICollection<AdminLog>? AdminLogs { get; set; } = new List<AdminLog>();
        public virtual ICollection<StaffLog>? StaffLogs { get; set; } = new List<StaffLog>();
        public virtual ICollection<StudentLog>? StudentLogs { get; set; } = new List<StudentLog>();
        public virtual ICollection<RoleLog>? RoleLogs { get; set; } = new List<RoleLog>();
        public virtual ICollection<PermissionLog>? PermissionLogs { get; set; } = new List<PermissionLog>();
        public virtual ICollection<SecurityEventLog>? SecurityEventLogs { get; set; } = new List<SecurityEventLog>();
        public virtual ICollection<StaffSuspensionLog>? StaffSuspensionLogs { get; set; } = new List<StaffSuspensionLog>();
        public virtual ICollection<StudentSuspensionLog>? StudentSuspensionLogs { get; set; } = new List<StudentSuspensionLog>();
        public virtual ICollection<AdminRoleLog>? AdminRoleLogs { get; set; } = new List<AdminRoleLog>();
        public virtual ICollection<StaffRoleLog>? StaffRoleLogs { get; set; } = new List<StaffRoleLog>();
        public virtual ICollection<StudentRoleLog>? StudentRoleLogs { get; set; } = new List<StudentRoleLog>();
        public virtual ICollection<AdminPermissionLog>? AdminPermissionLogs { get; set; } = new List<AdminPermissionLog>();
        public virtual ICollection<StaffPermissionLog>? StaffPermissionLogs { get; set; } = new List<StaffPermissionLog>();
        public virtual ICollection<StudentPermissionLog>? StudentPermissionLogs { get; set; } = new List<StudentPermissionLog>();
        public virtual ICollection<SecurityEventTypeLog>? SecurityEventTypeLogs { get; set; } = new List<SecurityEventTypeLog>();

    }
}
