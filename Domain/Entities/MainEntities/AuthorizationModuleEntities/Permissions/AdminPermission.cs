using Core.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.Permissions
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


