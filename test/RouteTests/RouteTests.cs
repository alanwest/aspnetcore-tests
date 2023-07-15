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

    public static IEnumerable<object[]> TestData => RouteTestData.GetTestCases();

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
    [MemberData(nameof(TestData))]
    public async Task<(string summary, string details)> ExampleTest(RouteTestData.RouteTestCase testCase)
    {
        this.app = TestApplicationFactory.CreateApplication(testCase.TestApplicationScenario);
        var _ = this.app.RunAsync();

        HttpResponseMessage? responseMessage = null;
        string response = string.Empty;

        responseMessage = await this.client.GetAsync(testCase.Path).ConfigureAwait(false);
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

        output?.WriteLine(string.Empty);
        output?.WriteLine($"Scenario={testCase.TestApplicationScenario} Path={testCase.Path}");

        var statusCode = activity.GetTagItem("http.status_code");
        Assert.Equal(testCase.ExpectedStatusCode, statusCode);

        output?.WriteLine(response);
        output?.WriteLine($"StatusCode={activity.GetTagItem("http.status_code")}");
        output?.WriteLine($"Activity.DisplayName={activity.DisplayName}");
        output?.WriteLine($"Activity.HttpRouteAttribute={activity.GetTagItem("http.route")}");

        return (Bloop(testCase, activity), response);
    }

    private string Bloop(RouteTestData.RouteTestCase testCase, Activity activity)
    {
        return $"| {string.Join(" | ",
            testCase.TestApplicationScenario,
            $"{testCase.HttpMethod} {testCase.ExpectedHttpRoute}",
            activity.DisplayName)} |";
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
