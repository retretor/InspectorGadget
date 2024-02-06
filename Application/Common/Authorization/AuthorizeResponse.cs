using Domain.Entities.Basic;

namespace Application.Common.Authorization;

public class AuthorizeResponse
{
    public string Token { get; init; }
    public DbUser User { get; init; }

    public AuthorizeResponse(string token, DbUser user)
    {
        Token = token;
        User = user;
    }
}