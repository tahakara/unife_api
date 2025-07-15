using Core.Entities.Base.Concrete;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Domain.Entities.MainEntities.AuthorizationModuleEntities.Suspensions
{
    public class StaffSuspension : BaseEntity
    {
        public Guid StaffSuspensionUuid { get; set; }
        public Guid StaffUuid { get; set; }
        public Guid? SuspendedByAdminUuid { get; set; }
        public Guid? SuspendedByStaffUuid { get; set; }
        public DateTime SuspensionStartDate { get; set; }
        public DateTime SuspensionEndDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public override DateTime CreatedAt { get; set; }
        public override DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Staff? Staff { get; set; }
        public virtual Admin? SuspendedByAdmin { get; set; }
        public virtual Staff? SuspendedByStaff { get; set; }
    }


}


