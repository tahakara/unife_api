using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Roles
{
    public class StudentRole : BaseEntity
    {
        public Guid StudentRoleUuid { get; set; }
        public Guid StudentUuid { get; set; }
        public Guid RoleUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Student? Student { get; set; }
        public virtual Role? Role { get; set; }
    }


}


