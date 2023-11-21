namespace InspectorGadget.Models;

public sealed partial class RepairType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();
}
