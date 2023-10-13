using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace RouteTests;

public enum TestApplicationScenario
{
    ConventionalRouting,
    AttributeRouting,
    MinimalApi,
    RazorPages,
}

internal class TestApplicationFactory
{
    private static string ContentRootPath = new FileInfo(typeof(RoutingTests)!.Assembly!.Location)!.Directory!.Parent!.Parent!.Parent!.FullName;

    public static WebApplication CreateApplication(TestApplicationScenario config)
    {
        switch (config)
        {
            case TestApplicationScenario.ConventionalRouting:
                return CreateConventionalRoutingApplication();
            case TestApplicationScenario.AttributeRouting:
                return CreateAttributeRoutingApplication();
            case TestApplicationScenario.MinimalApi:
                return CreateMinimalApiApplication();
            case TestApplicationScenario.RazorPages:
                return CreateRazorPagesApplication();
            default:
                throw new ArgumentException($"Invalid {nameof(TestApplicationScenario)}");
        }
    }

    private static WebApplication CreateConventionalRoutingApplication()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = ContentRootPath });
        builder.Services
            .AddControllersWithViews()
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseMiddleware<RouteInfoMiddleware>();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapAreaControllerRoute(
            name: "AnotherArea",
            areaName: "AnotherArea",
            pattern: "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "MyArea",
            pattern: "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}");

        app.MapControllerRoute(
            name: "FixedRouteWithConstraints",
            pattern: "SomePath/{id}/{num:int}",
            defaults: new { controller = "ConventionalRoute", action = "ActionWithStringParameter" });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=ConventionalRoute}/{action=Default}/{id?}");

        return app;
    }

    private static WebApplication CreateAttributeRoutingApplication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddControllers()
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseMiddleware<RouteInfoMiddleware>();
        app.MapControllers();

        return app;
    }

    private static WebApplication CreateMinimalApiApplication()
    {
        var builder = WebApplication.CreateSlimBuilder();

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseMiddleware<RouteInfoMiddleware>();

        var api = app.MapGroup("/MinimalApi");
        api.MapGet("/", () => Results.Ok());
        api.MapGet("/{id}", (int id) => Results.Ok());

        return app;
    }

    private static WebApplication CreateRazorPagesApplication()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = ContentRootPath });
        builder.Services
            .AddRazorPages()
            .AddRazorRuntimeCompilation(options =>
            {
                options.FileProviders.Add(new PhysicalFileProvider(ContentRootPath));
            })
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseMiddleware<RouteInfoMiddleware>();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();

        return app;
    }
}
