using Domain.Common;

namespace Domain.Entities.Basic;

public class AllowedRepairTypesForEmployee : BaseEntity
{
    public int RepairTypeForDeviceId { get; set; }
    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public virtual RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}