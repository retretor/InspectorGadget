namespace InspectorGadget.Models;

public sealed partial class Employee
{
    public int Id { get; set; }

    public int DbUserId { get; set; }

    public UserRole UserRole { get; set; }

    public ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } =
        new List<AllowedRepairTypesForEmployee>();

    public DbUser DbUser { get; set; } = null!;

    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}