using Domain.Common;

namespace Domain.Entities.Basic;

public class Employee : BaseEntity
{
    public int DbUserId { get; set; }

    public ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } =
        new List<AllowedRepairTypesForEmployee>();

    public DbUser DbUser { get; set; } = null!;
    public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}