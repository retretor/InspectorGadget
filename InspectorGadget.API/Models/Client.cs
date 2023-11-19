namespace InspectorGadget.Models;

public partial class Client
{
    public int Id { get; set; }

    public int DiscountPercentage { get; set; }

    public int DbUserId { get; set; }

    public virtual DbUser DbUser { get; set; } = null!;

    public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}
