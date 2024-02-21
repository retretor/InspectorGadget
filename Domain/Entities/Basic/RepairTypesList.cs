using Domain.Common;

namespace Domain.Entities.Basic;

public class RepairTypesList : BaseEntity
{
    public int RepairTypeForDeviceId { get; set; }
    public int RepairRequestId { get; set; }
    public virtual RepairRequest RepairRequest { get; set; } = null!;
    public virtual RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}