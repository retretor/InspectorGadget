namespace InspectorGadget.Models;

public sealed partial class Client
{
    public int Id { get; set; }

    public int DiscountPercentage { get; set; }

    public int DbUserId { get; set; }

    public DbUser DbUser { get; set; } = null!;

    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}
