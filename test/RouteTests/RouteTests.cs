using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Xunit.Abstractions;

namespace RouteTests;

public class RoutingTests : IDisposable
{
    private readonly ITestOutputHelper output;
    private TracerProvider tracerProvider;
    private WebApplication? app;
    private HttpClient client;
    private List<Activity> exportedItems;
    private AspNetCoreDiagnosticObserver diagnostics;

    public RoutingTests(ITestOutputHelper output)
    {
        this.output = output;
        this.diagnostics = new AspNetCoreDiagnosticObserver();
        this.exportedItems = new List<Activity>();
        this.client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
        this.tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .AddInMemoryExporter(this.exportedItems)
            .Build()!;
    }

    [Theory]
    [InlineData(TestApplicationType.ConventionalRouting, "/")]
    [InlineData(TestApplicationType.ConventionalRouting, "/ConventionalRoute/ActionWithStringParameter/2?num=3")]
    [InlineData(TestApplicationType.ConventionalRouting, "/ConventionalRoute/NotFound")]
    [InlineData(TestApplicationType.AttributeRouting, "/AttributeRoute")]
    [InlineData(TestApplicationType.AttributeRouting, "/AttributeRoute/Get")]
    [InlineData(TestApplicationType.AttributeRouting, "/AttributeRoute/Boop/asdf")]
    [InlineData(TestApplicationType.RazorPages, "/")]
    [InlineData(TestApplicationType.RazorPages, "/Index")]
    [InlineData(TestApplicationType.RazorPages, "/PageThatThrowsException")]
    public async void ExampleTest(TestApplicationType config, string path)
    {
        this.app = TestApplicationFactory.CreateApplication(config);
        this.app.RunAsync();

        HttpResponseMessage? responseMessage = null;
        string response = string.Empty;

        responseMessage = await this.client.GetAsync(path).ConfigureAwait(false);
        response = responseMessage.Content.ReadAsStringAsync().Result;

        for (var i = 0; i < 10; i++)
        {
            if (this.exportedItems.Count > 0)
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        }

        var activity = this.exportedItems[0];

        output.WriteLine(string.Empty);
        output.WriteLine($"Path={path}");
        output.WriteLine(response);
        output.WriteLine($"StatusCode={activity.GetTagItem("http.status_code")}");
        output.WriteLine($"Activity.DisplayName={activity.DisplayName}");
        output.WriteLine($"Activity.HttpRouteAttribute={activity.GetTagItem("http.route")}");

        // await this.client.GetStringAsync("http://localhost:5000").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/Default").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/ActionWithParameter").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/ActionWithParameter/2").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/ActionWithStringParameter").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/ActionWithStringParameter/2").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/ActionWithStringParameter/2?num=3").ConfigureAwait(false);

        // await this.client.GetStringAsync("http://localhost:5000/ControllerNotFound/Action").ConfigureAwait(false);
        // await this.client.GetStringAsync("http://localhost:5000/Conventional/ActionNotFound").ConfigureAwait(false);

        // var expectedActivityName = httpMethod + (!string.IsNullOrEmpty(expectedRoute) ? " " + expectedRoute : string.Empty);
        // Assert.Equal(expectedActivityName, activity.DisplayName);

        // var actualHttpRoute = activity.GetTagItem("http.route") as string;
        // Assert.Equal(expectedRoute, actualHttpRoute);
    }

    

    public async void Dispose()
    {
        this.tracerProvider.Dispose();
        this.diagnostics.Dispose();
        this.client.Dispose();
        if (this.app != null)
        {
            await this.app.DisposeAsync().ConfigureAwait(false);
        }
    }
}
