using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Roles
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


