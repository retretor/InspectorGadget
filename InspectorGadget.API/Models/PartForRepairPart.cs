namespace InspectorGadget.Models;

public partial class PartForRepairPart
{
    public int Id { get; set; }

    public int PartCount { get; set; }

    public int RepairTypeForDeviceId { get; set; }

    public int RepairPartId { get; set; }

    public virtual RepairPart RepairPart { get; set; } = null!;

    public virtual RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}
