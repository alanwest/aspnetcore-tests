using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace RouteTests;

public class RouteInfo
{
    public string? Path { get; private set; }
    public string? RawText { get; private set; }
    public string? RouteDiagnosticMetadata { get; private set; }
    public string? RouteData { get; private set; }
    public string? AttributeRouteInfo { get; private set; }
    public string? ActionParameters { get; private set; }
    public string? PageActionDescriptorRelativePath { get; private set; }
    public string? PageActionDescriptorViewEnginePath { get; private set; }
    public string? ControllerActionDescriptorControllerName { get; private set; }
    public string? ControllerActionDescriptorActionName { get; private set; }

    public void SetValues(HttpContext context)
    {
        this.Path = $"{context.Request.Path}{context.Request.QueryString}";

        var endpoint = context.GetEndpoint();

        this.RawText = (endpoint as RouteEndpoint)?.RoutePattern.RawText;
        this.RouteDiagnosticMetadata = endpoint?.Metadata.GetMetadata<IRouteDiagnosticsMetadata>()?.Route;

        var routeData = new List<string>();
        foreach (var item in context.GetRouteData().Values)
        {
            routeData.Add($"{item.Key}={item.Value}");
        }

        this.RouteData = string.Join(',', routeData);
    }

    public void SetValues(ActionDescriptor actionDescriptor)
    {
        this.AttributeRouteInfo = actionDescriptor.AttributeRouteInfo?.Template;
        this.ActionParameters = string.Join("/", actionDescriptor.Parameters.Select(p => "{" + p.Name + "}"));

        if (actionDescriptor is PageActionDescriptor pad)
        {
            this.PageActionDescriptorRelativePath = pad.RelativePath;
            this.PageActionDescriptorViewEnginePath = pad.ViewEnginePath;
        }
        
        if (actionDescriptor is ControllerActionDescriptor cad)
        {
            this.ControllerActionDescriptorControllerName = cad.ControllerName;
            this.ControllerActionDescriptorActionName = cad.ActionName;
        }
    }
}
