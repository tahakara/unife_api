using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base.Concrete
{
    public class AcademicDepartment : BaseEntity
    {
        public Guid DepartmentUuid { get; set; } = Guid.NewGuid();
        public int DepartmentTypeId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public string? DepartmentDescription { get; set; }
        public bool IsActive { get; set; } = true;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual AcademicDepartmentType? DepartmentType { get; set; }
        public virtual ICollection<UniversityFacultyDepartment> UniversityFacultyDepartments { get; set; } = new List<UniversityFacultyDepartment>();
    }
}
