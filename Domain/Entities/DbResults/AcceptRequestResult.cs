using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.DbResults;

public class AcceptRequestResult
{
    [Column("repair_request_id")] public int RepairRequestId { get; set; }
    [Column("status")] public string Status { get; set; } = null!;
    [Column("date")] public DateTime Date { get; set; }
    [Column("assigned_employee_id")] public int AssignedEmployeeId { get; set; }
}