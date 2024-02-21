using Domain.Common;

namespace Domain.Entities.Basic;

public class RequestStatusHistory : BaseEntity
{
    public DateTime Date { get; set; }
    public int RepairRequestId { get; set; }
    public string Status { get; set; } = null!;
    public virtual RepairRequest RepairRequest { get; set; } = null!;
}