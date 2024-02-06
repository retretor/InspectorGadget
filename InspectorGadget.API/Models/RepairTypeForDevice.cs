namespace InspectorGadget.Models;

public sealed class RepairTypeForDevice
{
    public int Id { get; set; }

    public int Cost { get; set; }

    public int Time { get; set; }

    public int RepairTypeId { get; set; }

    public int DeviceId { get; set; }

    public ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } =
        new List<AllowedRepairTypesForEmployee>();

    public Device Device { get; set; } = null!;

    public ICollection<PartForRepairPart> PartForRepairParts { get; set; } = new List<PartForRepairPart>();

    public RepairType RepairType { get; set; } = null!;

    public ICollection<RepairTypesList> RepairTypesLists { get; set; } = new List<RepairTypesList>();
}