using Domain.Common;

namespace Domain.Entities.Basic;

public class Client : BaseEntity
{

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string TelephoneNumber { get; set; } = null!;

    public int DiscountPercentage { get; set; }

    public int DbUserId { get; set; }

    public virtual DbUser DbUser { get; set; } = null!;

    public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}