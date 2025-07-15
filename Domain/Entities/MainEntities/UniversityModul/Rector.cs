using Core.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AcademicModulEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities.UniversityModul
{

    public class Rector : BaseEntity
    {
        public Guid RectorUuid { get; set; }
        public Guid UniversityUuid { get; set; }
        public Guid AcademicianUuid { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsActive { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual University? University { get; set; }
        public virtual Academician? Academician { get; set; }
    }
}
