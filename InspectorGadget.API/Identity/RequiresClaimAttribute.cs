using InspectorGadget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InspectorGadget.Identity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresClaimAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _claimName;
    private readonly object _claimValues;
    private readonly bool _isAllowSelfView;

    public RequiresClaimAttribute(string claimName, object claimValues, bool isAllowSelfView = false)
    {
        _claimName = claimName;
        _claimValues = claimValues;
        _isAllowSelfView = isAllowSelfView;
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

        if (!isClaimed && !_isAllowSelfView)
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
            UserRole[] roleArray => roleArray.Select(role => role.ToString()).ToArray(),
            UserRole role => new[] { role.ToString() },
            _ => null
        };
    }
}