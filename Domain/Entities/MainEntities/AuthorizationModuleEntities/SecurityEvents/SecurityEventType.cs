using Domain.Entities.Base.Concrete;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents
{
    public class SecurityEventType : BaseEntity
    {
        public Guid SecurityEventTypeUuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsSystemEvent { get; set; } = false;

        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<SecurityEvent>? SecurityEvents { get; set; }
    }


}
