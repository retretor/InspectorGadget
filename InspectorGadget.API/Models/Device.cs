namespace InspectorGadget.Models;

public sealed partial class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();

    public ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();

    public Device(string name)
    {
        Name = name;
    }
}
