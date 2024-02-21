using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Web.Controllers;

[Route("api")]
[ApiController]
public class InfoController : ControllerBase
{
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

    public InfoController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select(actionDescriptor => new
        {
            Action = actionDescriptor.RouteValues["Action"],
            Controller = actionDescriptor.RouteValues["Controller"],
            Parameters = actionDescriptor.Parameters.Select(p => new
            {
                p.Name,
                TypeName = p.ParameterType.Name,
                Properties = p.ParameterType.GetProperties()
                    .Where(prop => prop.Name != "DbContext")
                    .Select(prop => new
                    {
                        PropertyName = prop.Name,
                        PropertyType = prop.PropertyType.Name
                    })
            }),
            RouteTemplate = actionDescriptor.AttributeRouteInfo?.Template,
            HttpMethods = actionDescriptor.ActionConstraints
                ?.OfType<Microsoft.AspNetCore.Mvc.ActionConstraints.HttpMethodActionConstraint>().FirstOrDefault()
                ?.HttpMethods
        });

        return Ok(new
        {
            Name = "Inspector Gadget API",
            Version = "1.0.0",
            Routes = routes
        });
    }
}