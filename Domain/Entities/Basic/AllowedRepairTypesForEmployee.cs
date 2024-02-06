using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities.Basic;

[Table("allowed_repair_types_for_employee", Schema = "basic")]
public class AllowedRepairTypesForEmployee : BaseEntity
{
    [Column("allowed_repair_types_for_employee_pkey")]
    public int RepairTypeForDeviceId { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public RepairTypeForDevice RepairTypeForDevice { get; set; } = null!;
}