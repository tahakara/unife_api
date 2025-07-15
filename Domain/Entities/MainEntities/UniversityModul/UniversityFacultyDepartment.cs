using Core.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AcademicModulEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities.UniversityModul
{

    public class UniversityFacultyDepartment : BaseEntity
    {
        public Guid UniversityDepartmentUuid { get; set; } = Guid.NewGuid();
        public Guid UniversityUuid { get; set; }
        public Guid FacultyUuid { get; set; }
        public Guid DepartmentUuid { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual University University { get; set; } = null!;
        public virtual Faculty Faculty { get; set; } = null!;
        public virtual AcademicDepartment Department { get; set; } = null!;
        public virtual ICollection<Academician> Academicians { get; set; } = new List<Academician>();
    }
}
