namespace InspectorGadget.Models;

public sealed class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Series { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();

    public ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();
}