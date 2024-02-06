using Domain.Common;

namespace Domain.Entities.Basic;

public class RepairType : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();
}