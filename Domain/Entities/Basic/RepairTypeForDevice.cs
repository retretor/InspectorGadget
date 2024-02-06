using Domain.Common;

namespace Domain.Entities.Basic;

public class RepairTypeForDevice : BaseEntity
{
    public int Cost { get; set; }
    public int Time { get; set; }
    public int RepairTypeId { get; set; }
    public int DeviceId { get; set; }
    public ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } =
        new List<AllowedRepairTypesForEmployee>();
    public Device Device { get; set; } = null!;
    public ICollection<PartForRepairType> PartForRepairParts { get; set; } = new List<PartForRepairType>();
    public RepairType RepairType { get; set; } = null!;
    public ICollection<RepairTypesList> RepairTypesLists { get; set; } = new List<RepairTypesList>();
}