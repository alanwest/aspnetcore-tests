using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace RouteTests;

public class DebugInfo
{
    public string? RawText { get; set; }
    public string? RouteDiagnosticMetadata { get; set; }
    public IDictionary<string, string?>? RouteData { get; set; }
    public string? AttributeRouteInfo { get; set; }
    public IList<string>? ActionParameters { get; set; }
    public string? PageActionDescriptorRelativePath { get; set; }
    public string? PageActionDescriptorViewEnginePath { get; set; }
    public string? ControllerActionDescriptorControllerName { get; set; }
    public string? ControllerActionDescriptorActionName { get; set; }
}

public class RouteInfo
{
    private static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
 
    public RouteInfo()
    {
        DebugInfo = new DebugInfo();
    }

    public string? HttpMethod { get; set; }
    public string? Path { get; set; }

    public string? HttpRouteByRawText => this.DebugInfo.RawText;

    public string HttpRouteByControllerActionAndParameters
    {
        get
        {
            var condition = !string.IsNullOrEmpty(this.DebugInfo.ControllerActionDescriptorActionName)
                && !string.IsNullOrEmpty(this.DebugInfo.ControllerActionDescriptorActionName)
                && this.DebugInfo.ActionParameters != null;

            if (!condition)
            {
                return string.Empty;
            }

            var paramList = string.Join(string.Empty, this.DebugInfo.ActionParameters!.Select(p => $"/{{{p}}}"));
            return $"/{this.DebugInfo.ControllerActionDescriptorControllerName}/{this.DebugInfo.ControllerActionDescriptorActionName}{paramList}";
        }
    }

    public string HttpRouteByActionDescriptor
    {
        get
        {
            var result = string.Empty;

            var hasControllerActionDescriptor = this.DebugInfo.ControllerActionDescriptorControllerName != null
                && this.DebugInfo.ControllerActionDescriptorActionName != null;

            var hasPageActionDescriptor = this.DebugInfo.PageActionDescriptorRelativePath != null
                && this.DebugInfo.PageActionDescriptorViewEnginePath != null;

            if (this.DebugInfo.RawText != null && hasControllerActionDescriptor)
            {
                var controllerRegex = new System.Text.RegularExpressions.Regex(@"\{controller=.*?\}+?");
                var actionRegex = new System.Text.RegularExpressions.Regex(@"\{action=.*?\}+?");
                result = controllerRegex.Replace(this.DebugInfo.RawText, this.DebugInfo.ControllerActionDescriptorControllerName!);
                result = actionRegex.Replace(result, this.DebugInfo.ControllerActionDescriptorActionName!);
            }
            else if (this.DebugInfo.RawText != null && hasPageActionDescriptor)
            {
                result = this.DebugInfo.PageActionDescriptorRelativePath!;
            }

            return result;
        }
    }

    public DebugInfo DebugInfo { get; set; }

    public void SetValues(HttpContext context)
    {
        this.HttpMethod = context.Request.Method;
        this.Path = $"{context.Request.Path}{context.Request.QueryString}";
        var endpoint = context.GetEndpoint();
        this.DebugInfo.RawText = (endpoint as RouteEndpoint)?.RoutePattern.RawText;
        this.DebugInfo.RouteDiagnosticMetadata = endpoint?.Metadata.GetMetadata<IRouteDiagnosticsMetadata>()?.Route;
        this.DebugInfo.RouteData = new Dictionary<string, string?>();
        foreach (var value in context.GetRouteData().Values)
        {
            this.DebugInfo.RouteData[value.Key] = value.Value?.ToString();
        }
    }

    public void SetValues(ActionDescriptor actionDescriptor)
    {
        this.DebugInfo.AttributeRouteInfo = actionDescriptor.AttributeRouteInfo?.Template;

        this.DebugInfo.ActionParameters = new List<string>();
        foreach (var item in actionDescriptor.Parameters)
        {
            this.DebugInfo.ActionParameters.Add(item.Name);
        }

        if (actionDescriptor is PageActionDescriptor pad)
        {
            this.DebugInfo.PageActionDescriptorRelativePath = pad.RelativePath;
            this.DebugInfo.PageActionDescriptorViewEnginePath = pad.ViewEnginePath;
        }
        
        if (actionDescriptor is ControllerActionDescriptor cad)
        {
            this.DebugInfo.ControllerActionDescriptorControllerName = cad.ControllerName;
            this.DebugInfo.ControllerActionDescriptorActionName = cad.ActionName;
        }
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, JsonSerializerOptions);
    }
}
