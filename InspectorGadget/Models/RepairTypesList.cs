namespace InspectorGadget.Models;

public sealed partial class RepairTypesList
{
    public int Id { get; set; }

    public int RepairTypeForDeviceId { get; set; }

    public int RepairRequestId { get; set; }

    public RepairRequest RepairRequest { get; set; } = null!;

    public RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}
