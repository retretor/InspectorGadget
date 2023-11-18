namespace InspectorGadget.Models;

public sealed partial class AllowedRepairTypesForEmployee
{
    public int Id { get; set; }

    public int RepairTypeForDeviceId { get; set; }

    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}
