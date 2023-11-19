namespace InspectorGadget.DTOs;

public class AuthorizeResponse
{
    public string Token { get; set; } = null!;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = null!;
    public string ClientType { get; set; } = null!;
}