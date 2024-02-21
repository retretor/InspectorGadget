using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities.Basic;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authorization;

public class JwtService
{
    private readonly string _secret;
    private readonly int _expirationInMinutes;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(IConfiguration configuration)
    {
        _secret = configuration["JwtSettings:Key"]!;
        _expirationInMinutes = int.Parse(configuration["JwtSettings:MinutesToExpiration"]!);
        _issuer = configuration["JwtSettings:Issuer"]!;
        _audience = configuration["JwtSettings:Audience"]!;
    }

    public string GenerateToken(DbUser user, DateTime? validTo = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Login),
            new(ClaimTypes.Role, user.Role),
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.UserData, user.SecretKey)
        };

        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: validTo ?? DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expirationInMinutes)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public bool IsValidToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }
}