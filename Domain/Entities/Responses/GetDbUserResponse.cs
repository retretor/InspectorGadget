namespace Domain.Entities.Responses;

public class GetDbUserResponse
{
    // TODO: maybe delete SecretKey from here
    public int EntityId { get; init; }
    public int? ClientId { get; init; }
    public int? EmployeeId { get; init; }
    public string Login { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string Role { get; set; } = null!;
}