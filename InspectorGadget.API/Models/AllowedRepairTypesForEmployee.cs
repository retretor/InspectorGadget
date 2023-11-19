namespace InspectorGadget.Models;

public partial class AllowedRepairTypesForEmployee
{
    public int Id { get; set; }

    public int RepairTypeForDeviceId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}
