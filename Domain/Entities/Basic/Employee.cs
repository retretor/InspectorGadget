using Domain.Common;

namespace Domain.Entities.Basic;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string TelephoneNumber { get; set; } = null!;
    public int ExperienceYears { get; set; }
    public int YearsInCompany { get; set; }
    public int Rating { get; set; }
    public int DbUserId { get; set; }
    public string PhotoPath { get; set; } = null!;

    public virtual ICollection<AllowedRepairTypesForEmployee> AllowedRepairTypesForEmployees { get; set; } =
        new List<AllowedRepairTypesForEmployee>();

    public virtual DbUser DbUser { get; set; } = null!;

    public virtual ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();
}