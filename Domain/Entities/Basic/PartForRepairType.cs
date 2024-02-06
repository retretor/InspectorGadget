using Domain.Common;

namespace Domain.Entities.Basic;

public class PartForRepairType : BaseEntity
{
    public int PartCount { get; set; }
    public int RepairTypeForDeviceId { get; set; }
    public int RepairPartId { get; set; }
    public RepairPart RepairPart { get; set; } = null!;
    public RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}