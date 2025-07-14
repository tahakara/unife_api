using Domain.Entities.Base.Concrete;

namespace Domain.Entities.AuthorizationEntities.Suspensions
{
    public class StudentSuspension : BaseEntity
    {
        public Guid StudentSuspensionUuid { get; set; }
        public Guid StudentUuid { get; set; }
        public Guid? SuspendedByAdminUuid { get; set; } // Admin who suspended the student
        public Guid? SuspendedByStaffUuid { get; set; } // Staff who suspended the student
        public Guid? SuspendedByStudentUuid { get; set; } // Student who suspended the student (if applicable)
        public DateTime SuspensionStartDate { get; set; }
        public DateTime SuspensionEndDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Student? Student { get; set; }
        public virtual Admin? SuspendedByAdmin { get; set; }
        public virtual Staff? SuspendedByStaff { get; set; }
        public virtual Student? SuspendedByStudent { get; set; }
    }


}


