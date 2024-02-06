using Domain.Common;

namespace Domain.Entities.Basic;

public class Client : BaseEntity
{
    public int DiscountPercentage { get; set; }
    public int DbUserId { get; set; }
    public DbUser DbUser { get; set; } = null!;
    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}