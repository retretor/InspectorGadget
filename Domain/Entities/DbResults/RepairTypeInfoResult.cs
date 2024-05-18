using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class RepairTypeInfoResult
{
    [Column("repair_type_id")] public int RepairTypeId { get; set; }
    [Column("repair_type_name")] public string RepairTypeName { get; set; } = null!;
    [Column("cost")] public int Cost { get; set; }
    [Column("days_to_complete")] public int DaysToComplete { get; set; }
}