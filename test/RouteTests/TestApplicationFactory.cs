using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace RouteTests;

public enum TestApplicationType
{
    ConventionalRouting,
    AttributeRouting,
    RazorPages,
}

internal class TestApplicationFactory
{
    public static WebApplication CreateApplication(TestApplicationType config)
    {
        switch (config)
        {
            case TestApplicationType.ConventionalRouting:
                return CreateConventionalRoutingApplication();
            case TestApplicationType.AttributeRouting:
                return CreateAttributeRoutingApplication();
            case TestApplicationType.RazorPages:
                return CreateRazorPagesApplication();
            default:
                throw new ArgumentException($"Invalid {nameof(TestApplicationType)}");
        }
    }

    private static WebApplication CreateConventionalRoutingApplication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddControllersWithViews()
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseStaticFiles();
        app.UseMiddleware<RouteInfoMiddleware>();
        app.UseRouting();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Conventional}/{action=Default}/{id?}");

        return app;
    }

    private static WebApplication CreateAttributeRoutingApplication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseMiddleware<RouteInfoMiddleware>();
        app.MapControllers();

        return app;
    }

    private static WebApplication CreateRazorPagesApplication()
    {
        var contentRoot = new FileInfo(typeof(RoutingTests).Assembly.Location).Directory.Parent.Parent.Parent.FullName;

        var builder = WebApplication.CreateBuilder();
        builder.Services
            .AddRazorPages()
            .AddRazorRuntimeCompilation(options =>
            {
                options.FileProviders.Add(new PhysicalFileProvider(contentRoot));
            })
            .AddApplicationPart(typeof(RoutingTests).Assembly);;

        var app = builder.Build();
        app.UseExceptionHandler(RouteInfoMiddleware.ConfigureExceptionHandler);
        app.UseStaticFiles();
        app.UseMiddleware<RouteInfoMiddleware>();
        app.UseRouting();
        app.MapRazorPages();

        return app;
    }
}
