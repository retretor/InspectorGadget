namespace InspectorGadget.Models;

public partial class RepairTypesList
{
    public int Id { get; set; }

    public int RepairTypeForDeviceId { get; set; }

    public int RepairRequestId { get; set; }

    public virtual RepairRequest RepairRequest { get; set; } = null!;

    public virtual RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}
