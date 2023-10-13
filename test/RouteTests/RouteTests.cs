using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace RouteTests;

public class RoutingTests : IDisposable
{
    private TracerProvider tracerProvider;
    private MeterProvider meterProvider;
    private WebApplication? app;
    private HttpClient client;
    private List<Activity> exportedActivities;
    private List<Metric> exportedMetrics;
    private AspNetCoreDiagnosticObserver diagnostics;

    private static string HttpStatusCode = "http.status_code";
    private static string HttpMethod = "http.method";
    private static string HttpRoute = "http.route";

    public static IEnumerable<object[]> TestData => RouteTestData.GetTestCases();

    public RoutingTests()
    {
        this.diagnostics = new AspNetCoreDiagnosticObserver();
        this.client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

        this.exportedActivities = new List<Activity>();
        this.exportedMetrics = new List<Metric>();

        this.tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .AddInMemoryExporter(this.exportedActivities)
            .Build()!;

        this.meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .AddInMemoryExporter(this.exportedMetrics)
            .Build()!;
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task<TestResult> TestRoutes(RouteTestData.RouteTestCase testCase, bool skipAsserts = true)
    {
        this.app = TestApplicationFactory.CreateApplication(testCase.TestApplicationScenario);
        var _ = this.app.RunAsync();

        var responseMessage = await this.client.GetAsync(testCase.Path).ConfigureAwait(false);
        var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        var info = JsonSerializer.Deserialize<RouteInfo>(response);

        for (var i = 0; i < 10; i++)
        {
            if (this.exportedActivities.Count > 0)
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        }

        this.meterProvider.ForceFlush();

        Assert.Single(this.exportedActivities);
        Assert.Single(this.exportedMetrics);
        
        var metricPoints = new List<MetricPoint>();
        foreach (var mp in this.exportedMetrics[0].GetMetricPoints())
        {
            metricPoints.Add(mp);
        }

        Assert.Single(metricPoints);

        var activity = this.exportedActivities[0];
        var metricPoint = metricPoints.First();

        GetTagsFromActivity(activity, out var activityHttpStatusCode, out var activityHttpMethod, out var activityHttpRoute);
        GetTagsFromMetricPoint(metricPoint, out var metricHttpStatusCode, out var metricHttpMethod, out var metricHttpRoute);

        Assert.Equal(testCase.ExpectedStatusCode, activityHttpStatusCode);
        Assert.Equal(testCase.ExpectedStatusCode, metricHttpStatusCode);
        Assert.Equal(testCase.HttpMethod, activityHttpMethod);
        Assert.Equal(testCase.HttpMethod, metricHttpMethod);

        if (!skipAsserts)
        {
            Assert.Equal(testCase.ExpectedHttpRoute, activityHttpRoute);
            Assert.Equal(testCase.ExpectedHttpRoute, metricHttpRoute);

            var expectedActivityDisplayName = string.IsNullOrEmpty(testCase.ExpectedHttpRoute)
                ? testCase.HttpMethod
                : $"{testCase.HttpMethod} {testCase.ExpectedHttpRoute}";
            Assert.Equal(expectedActivityDisplayName, activity.DisplayName);
        }

        return new TestResult
        {
            ActivityDisplayName = activity.DisplayName,
            HttpStatusCode = activityHttpStatusCode,
            HttpMethod = activityHttpMethod,
            HttpRoute = activityHttpRoute,
            RouteInfo = info!,
            TestCase = testCase,
        };
    }

    public async void Dispose()
    {
        this.tracerProvider.Dispose();
        this.meterProvider.Dispose();
        this.diagnostics.Dispose();
        this.client.Dispose();
        if (this.app != null)
        {
            await this.app.DisposeAsync().ConfigureAwait(false);
        }
    }

    private void GetTagsFromActivity(Activity activity, out int httpStatusCode, out string httpMethod, out string? httpRoute)
    {
        httpStatusCode = Convert.ToInt32(activity.GetTagItem(HttpStatusCode));
        httpMethod = (activity.GetTagItem(HttpMethod) as string)!;
        httpRoute = activity.GetTagItem(HttpRoute) as string ?? string.Empty;
    }

    private void GetTagsFromMetricPoint(MetricPoint metricPoint, out int httpStatusCode, out string httpMethod, out string? httpRoute)
    {
        httpStatusCode = 0;
        httpMethod = string.Empty;
        httpRoute = string.Empty;

        foreach (var tag in metricPoint.Tags)
        {
            if (tag.Key.Equals(HttpStatusCode))
            {
                httpStatusCode = Convert.ToInt32(tag.Value);
            }
            else if (tag.Key.Equals(HttpMethod))
            {
                httpMethod = (tag.Value as string)!;
            }
            else if (tag.Key.Equals(HttpRoute))
            {
                httpRoute = tag.Value as string;
            }
        }
    }
}
