namespace InspectorGadget.Models;

public sealed partial class PartForRepairPart
{
    public int Id { get; set; }

    public int PartCount { get; set; }

    public int RepairTypeForDeviceId { get; set; }

    public int RepairPartId { get; set; }

    public RepairPart RepairPart { get; set; } = null!;

    public RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}
