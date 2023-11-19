namespace InspectorGadget.Models;

public partial class RepairTypeForDevice
{
    public int Id { get; set; }

    public int Cost { get; set; }

    public int Time { get; set; }

    public int RepairTypeId { get; set; }

    public int DeviceId { get; set; }

    public virtual ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } = new List<AllowedRepairTypesForEmployee>();

    public virtual Device Device { get; set; } = null!;

    public virtual ICollection<PartForRepairPart> PartForRepairParts { get; set; } = new List<PartForRepairPart>();

    public virtual RepairType RepairType { get; set; } = null!;

    public virtual ICollection<RepairTypesList> RepairTypesLists { get; set; } = new List<RepairTypesList>();
}
