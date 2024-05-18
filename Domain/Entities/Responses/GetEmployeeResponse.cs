namespace Domain.Entities.Responses;

public class GetEmployeeResponse
{
    public int EntityId { get; init; }
    public int UserId { get; init; }
    public string FirstName { get; init; } = null!;
    public string SecondName { get; init; } = null!;
    public string TelephoneNumber { get; init; } = null!;
    public int ExperienceYears { get; init; }
    public int YearsInCompany { get; init; }
    public int Rating { get; init; }
    public string PhotoPath { get; init; } = null!;
}