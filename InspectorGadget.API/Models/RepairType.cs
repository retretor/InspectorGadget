namespace InspectorGadget.Models;

public partial class RepairType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RepairTypeForDevice> RepairTypeForDevices { get; set; } = new List<RepairTypeForDevice>();
}
