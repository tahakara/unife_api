using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Core.Entities.Base.Concrete;

namespace Domain.Entities.MainEntities.UniversityModul
{
    public class UniversityType : BaseEntity
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? TypeDescription { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<University> Universities { get; set; } = new List<University>();
    }
}
