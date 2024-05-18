namespace Domain.Entities.Responses;

public class GetClientResponse
{
    public int EntityId { get; init; }
    public int DbUserId { get; init; }
    public string FirstName { get; init; } = null!;
    public string SecondName { get; init; } = null!;
    public string TelephoneNumber { get; init; } = null!;
    public int DiscountPercentage { get; init; }
}