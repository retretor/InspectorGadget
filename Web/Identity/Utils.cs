using System.Security.Claims;
using Domain.Enums;

namespace Web.Identity;

public static class Utils
{
    public static Role GetUserRole(ClaimsPrincipal user)
    {
        var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? Role.ANONYMOUS.ToString();
        return Enum.Parse<Role>(role);
    }
}