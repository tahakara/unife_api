using Domain.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.Roles
{
    public class StaffRole : BaseEntity
    {
        public Guid StaffRoleUuid { get; set; }
        public Guid StaffUuid { get; set; }
        public Guid RoleUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Staff? Staff { get; set; }
        public virtual Role? Role { get; set; }
    }


}


