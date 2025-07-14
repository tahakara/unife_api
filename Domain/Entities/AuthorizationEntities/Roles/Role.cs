using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Roles
{
    public class Role : BaseEntity
    {
        public Guid RoleUuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsEditable { get; set; } = true;
        public bool IsSystemRole { get; set; } = false;
        public Guid? CreatorAdminUuid { get; set; }
        public Guid? CreatorStaffUuid { get; set; }
        public Guid? CreatorStudentUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        //public virtual ICollection<Permission>? Permissions { get; set; }
        //public virtual ICollection<Admin>? Admins { get; set; }
        //public virtual ICollection<Staff>? StaffMembers { get; set; }
        //public virtual ICollection<Student>? Students { get; set; }

        public virtual ICollection<AdminRole>? AdminRoles { get; set; }
        public virtual ICollection<StaffRole>? StaffRoles { get; set; }
        public virtual ICollection<StudentRole>? StudentRoles { get; set; }

        // Creator navigation properties
        public virtual Admin? CreatorAdmin { get; set; }
        public virtual Staff? CreatorStaff { get; set; }
        public virtual Student? CreatorStudent { get; set; }
    }


}


