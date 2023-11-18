namespace InspectorGadget.Models;

public sealed partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string TelephoneNumber { get; set; } = null!;

    public ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } = new List<AllowedRepairTypesForEmployee>();

    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}
