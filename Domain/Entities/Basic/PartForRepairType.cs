using Domain.Common;

namespace Domain.Entities.Basic;

public class PartForRepairType : BaseEntity
{
    public int PartCount { get; set; }
    public int RepairTypeForDeviceId { get; set; }
    public int RepairPartId { get; set; }
    public virtual RepairPart RepairPart { get; set; } = null!;
    public virtual RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}