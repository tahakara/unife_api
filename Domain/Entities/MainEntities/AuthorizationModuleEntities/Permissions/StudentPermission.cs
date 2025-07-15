using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.Permissions
{
    public class StudentPermission : BaseEntity
    {
        public Guid StudentPermissionUuid { get; set; }
        public Guid StudentUuid { get; set; }
        public Guid PermissionUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Student? Student { get; set; }
        public virtual Permission? Permission { get; set; }
    }


}


