using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base.Concrete
{
    public class AcademicDepartmentType : BaseEntity
    {
        public int TypeId { get; set; }
        public string TypeShortName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string? TypeDescription { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<AcademicDepartment> AcademicDepartments { get; set; } = new List<AcademicDepartment>();
    }
}
