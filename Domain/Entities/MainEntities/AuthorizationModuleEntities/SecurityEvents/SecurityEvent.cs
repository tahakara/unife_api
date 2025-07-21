using Core.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Entities.MainEntities.UniversityModul;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents
{
    public class SecurityEvent : BaseEntity
    {
        public Guid SecurityEventUuid { get; set; } = Guid.NewGuid();
        public Guid? EventTypeUuid { get; set; }
        public Guid? UniversityUuid { get; set; } 
        public Guid? EventedByAdminUuid { get; set; }
        public Guid? EventedByStaffUuid { get; set; }
        public Guid? EventedByStudentUuid { get; set; }
        public string Description { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string? UserAgent { get; set; }
        public string? AdditionalData { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual SecurityEventType? EventType { get; set; }
        public virtual University? University { get; set; }
        public virtual Admin? EventedByAdmin { get; set; }
        public virtual Staff? EventedByStaff { get; set; }
        public virtual Student? EventedByStudent { get; set; }
    }


}


