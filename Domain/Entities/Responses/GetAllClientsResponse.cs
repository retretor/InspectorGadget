namespace Domain.Entities.Responses;

public class GetAllClientsResponse
{
    public IEnumerable<GetClientResponse> Clients { get; init; } = null!;
}