using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities.AcademicModulEntities
{

    public class AcademicianTitle : BaseEntity
    {
        public int TitleId { get; set; }
        public string TitleShortName { get; set; } = string.Empty;
        public string TitleName { get; set; } = string.Empty;
        public string? TitleDescription { get; set; }
        public int TitleOrder { get; set; } = 0;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Academician> Academicians { get; set; } = new List<Academician>();
    }
}
