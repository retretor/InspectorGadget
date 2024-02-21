using Domain.Common;

namespace Domain.Entities.Basic;

public class RepairPart : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Specification { get; set; } = null!;
    public int CurrentCount { get; set; }
    public int MinAllowedCount { get; set; }
    public int Cost { get; set; }
    public string Condition { get; set; } = null!;
    public virtual ICollection<PartForRepairType> PartForRepairTypes { get; set; } = new List<PartForRepairType>();
}