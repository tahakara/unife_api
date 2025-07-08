using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base.Concrete
{

    public class UniversityCommunication : BaseEntity
    {
        public Guid CommunicationUuid { get; set; }
        public Guid UniversityUuid { get; set; }
        public int TypeId { get; set; }
        public string CommunicationValue { get; set; } = string.Empty;
        public int Priority { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual University University { get; set; } = null!;
        public virtual CommunicationType Type { get; set; } = null!;
    }
}
