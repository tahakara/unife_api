using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Permissions
{
    public class Permission : BaseEntity
    {
        public Guid PermissionUuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsEditable { get; set; } = true;
        public bool IsSystemPermission { get; set; } = false;
        public Guid? CreatorAdminUuid { get; set; }
        public Guid? CreatorStaffUuid { get; set; } // CreatorPersonelUuid yerine CreatorStaffUuid
        public Guid? CreatorStudentUuid { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        //public virtual ICollection<Role>? Roles { get; set; }
        //public virtual ICollection<Admin>? Admins { get; set; }
        //public virtual ICollection<Staff>? StaffMembers { get; set; }
        //public virtual ICollection<Student>? Students { get; set; }

        public virtual ICollection<AdminPermission>? AdminPermissions { get; set; }
        public virtual ICollection<StaffPermission>? StaffPermissions { get; set; }
        public virtual ICollection<StudentPermission>? StudentPermissions { get; set; }

        // Creator navigation properties
        public virtual Admin? CreatorAdmin { get; set; }
        public virtual Staff? CreatorStaff { get; set; }
        public virtual Student? CreatorStudent { get; set; }
    }


}


