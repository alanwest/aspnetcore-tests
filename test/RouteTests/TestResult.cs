namespace RouteTests;

public class TestResult
{
    public string ActivityDisplayName { get; set; } = string.Empty;
    public int HttpStatusCode { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string? HttpRoute { get; set; }
    public RouteInfo RouteInfo { get; set; } = new RouteInfo();
    public RouteTestData.RouteTestCase TestCase { get; set; } = new RouteTestData.RouteTestCase();
}
