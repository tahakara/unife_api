using Domain.Entities.Base.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base.Concrete
{

    public class AddressType : BaseEntity
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? TypeDescription { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<UniversityAddress> UniversityAddresses { get; set; } = new List<UniversityAddress>();
    }
}
