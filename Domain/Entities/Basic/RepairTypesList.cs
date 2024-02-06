using Domain.Common;

namespace Domain.Entities.Basic;

public class RepairTypesList : BaseEntity
{
    public int RepairTypeForDeviceId { get; set; }
    public int RepairRequestId { get; set; }
    public RepairRequest RepairRequest { get; set; } = null!;
    public RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}