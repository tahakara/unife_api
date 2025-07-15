using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.UniversityModul;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MainEntities
{

    public class CommunicationType : BaseEntity
    {
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? TypeDescription { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual CommunicationCategory? Category { get; set; }
        public virtual ICollection<UniversityCommunication> UniversityCommunications { get; set; } = new List<UniversityCommunication>();
    }
}
