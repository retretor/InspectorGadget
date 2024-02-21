using Domain.Common;

namespace Domain.Entities.Basic;

public class RepairTypeForDevice : BaseEntity
{
    public int Cost { get; set; }
    public int DaysToComplete { get; set; }
    public int RepairTypeId { get; set; }
    public int DeviceId { get; set; }
    public virtual ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } =
        new List<AllowedRepairTypesForEmployee>();
    public virtual Device Device { get; set; } = null!;
    public virtual ICollection<PartForRepairType> PartForRepairTypes { get; set; } = new List<PartForRepairType>();
    public virtual RepairType RepairType { get; set; } = null!;
    public virtual ICollection<RepairTypesList> RepairTypesLists { get; set; } = new List<RepairTypesList>();
}