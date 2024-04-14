namespace Application.Common.Authorization;

public class AuthorizeResponse
{
    public string Token { get; init; }

    public AuthorizeResponse(string token)
    {
        Token = token;
    }
}