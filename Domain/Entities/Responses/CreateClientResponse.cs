namespace Domain.Entities.Responses;

public class CreateClientResponse
{
    public int DbUserId { get; init; }
    public string? Token { get; init; }
}