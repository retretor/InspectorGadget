namespace InspectorGadget.Models;

public sealed partial class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string TelephoneNumber { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int DiscountPercentage { get; set; }

    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}
