using Domain.Common;

namespace Domain.Entities.Basic;

public class Device : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string Series { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
    public ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();
}