using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Routing;

namespace RouteTests;

public class RouteInfo
{
    public string? RawText { get; private set; }
    public string? RouteDiagnosticMetadata { get; private set; }
    public string? RouteData { get; private set; }
    public string? AttributeRouteInfo { get; private set; }
    public string? ActionParameters { get; private set; }

    public void SetValues(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        this.RawText = (endpoint as RouteEndpoint)?.RoutePattern.RawText;
        this.RouteDiagnosticMetadata = endpoint?.Metadata.GetMetadata<IRouteDiagnosticsMetadata>()?.Route;

        var sb = new System.Text.StringBuilder();
        foreach (var item in context.GetRouteData().Values)
        {
            sb.Append($"/{item.Key}={item.Value}");
            if (item.Key.Equals("controller", StringComparison.InvariantCultureIgnoreCase)
                || item.Key.Equals("action", StringComparison.InvariantCultureIgnoreCase))
            {
            }
        }

        this.RouteData = sb.ToString();
    }

    public void SetValues(BeforeActionEventData data)
    {
        var actionDescriptor = data.ActionDescriptor;
        this.AttributeRouteInfo = actionDescriptor.AttributeRouteInfo?.Template;
        this.ActionParameters = string.Join("/", actionDescriptor.Parameters.Select(p => "{" + p.Name + "}"));
    }
}
