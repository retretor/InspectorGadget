using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InspectorGadget.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace InspectorGadget.Services.AuthServices;

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
    
    public string GenerateToken(DbUserAuthDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        var jwt = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expirationInMinutes)),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}