namespace InspectorGadget.DTOs;

public class AuthorizeResponse
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string ClientType { get; set; } = null!;

    public AuthorizeResponse()
    {
    }

    public AuthorizeResponse(string token, int id, string clientName, string clientType)
    {
        Token = token;
        Id = id;
        ClientName = clientName;
        ClientType = clientType;
    }
}