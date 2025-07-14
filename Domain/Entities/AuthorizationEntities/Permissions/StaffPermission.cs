using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Permissions
{
    public class StaffPermission : BaseEntity
    {
        public Guid StaffPermissionUuid { get; set; }
        public Guid StaffUuid { get; set; }
        public Guid PermissionUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Staff? Staff { get; set; }
        public virtual Permission? Permission { get; set; }
    }


}


