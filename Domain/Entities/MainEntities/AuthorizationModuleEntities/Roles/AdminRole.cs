using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.Roles
{
    public class AdminRole : BaseEntity
    {
        public Guid AdminRoleUuid { get; set; }
        public Guid AdminUuid { get; set; }
        public Guid RoleUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Admin? Admin { get; set; }
        public virtual Role? Role { get; set; }
    }


}


