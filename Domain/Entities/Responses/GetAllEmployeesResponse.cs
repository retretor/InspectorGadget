namespace Domain.Entities.Responses;

public class GetAllEmployeesResponse
{
    public IEnumerable<GetEmployeeResponse> Employees { get; init; } = null!;
}