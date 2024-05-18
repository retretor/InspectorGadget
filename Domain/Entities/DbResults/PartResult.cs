using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class PartResult
{
    [Column("part_id")] public int PartId { get; set; }
    [Column("part_name")] public string PartName { get; set; } = null!;
    [Column("part_condition")] public string PartCondition { get; set; } = null!;
    [Column("part_specification")] public string PartSpecification { get; set; } = null!;
    [Column("part_current_count")] public int PartCurrentCount { get; set; }
    [Column("part_min_allowed_count")] public int PartMinAllowedCount { get; set; }
    [Column("part_cost")] public int PartCost { get; set; }
    [Column("device_id")] public int? DeviceId { get; set; }
}