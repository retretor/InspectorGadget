using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Identity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresClaimAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _claimName;
    private readonly object _claimValues;

    public RequiresClaimAttribute(string claimName, object claimValues)
    {
        _claimName = claimName;
        _claimValues = claimValues;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var claimValues = CastClaimValues();
        if (claimValues == null)
        {
            context.Result = new BadRequestResult();
            return;
        }

        var isClaimed = false;
        foreach (var claimValue in claimValues)
        {
            // TODO: Remove this debug code
            Console.WriteLine("ClaimValue: " + claimValue);
            Console.WriteLine("UserValue: " + user.FindFirst(_claimName)?.Value);
            isClaimed = user.HasClaim(_claimName, claimValue);
            if (isClaimed) break;
        }

        if (!isClaimed)
        {
            context.Result = new ForbidResult();
        }
    }

    private string[]? CastClaimValues()
    {
        return _claimValues switch
        {
            string[] stringArray => stringArray,
            string stringValue => new[] { stringValue },
            Role[] roleArray => roleArray.Select(role => role.ToString()).ToArray(),
            Role role => new[] { role.ToString() },
            _ => null
        };
    }
}