namespace InspectorGadget.Models;

public partial class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();

    public virtual ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();
}
