using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Permissions
{
    public class AdminPermission : BaseEntity
    {
        public Guid AdminPermissionUuid { get; set; }
        public Guid AdminUuid { get; set; }
        public Guid PermissionUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Admin? Admin { get; set; }
        public virtual Permission? Permission { get; set; }
    }


}


