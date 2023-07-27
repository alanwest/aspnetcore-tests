using System.Text;
using RouteTests;

var sb = new StringBuilder();

var testCases = RoutingTests.TestData;

var results = new List<TestResult>();

foreach (var item in testCases)
{
    using var tests = new RoutingTests();
    var testCase = item[0] as RouteTestData.RouteTestCase;
    var result = await tests.TestRoutes(testCase!);
    results.Add(result);
}

sb.AppendLine("| | Scenario | Request | Activity.DisplayName | Expected http.route | Strategy 1 | Strategy 2 | Strategy 3 |");
sb.AppendLine("| - | - | - | - | - | - | - | - |");

for (var i = 0; i < results.Count; ++i)
{
    sb.Append($"| [{i + 1}](#{i + 1}) ");
    sb.AppendLine(FormatTestResult(results[i]));
}

for (var i = 0; i < results.Count; ++i)
{
    sb.AppendLine();
    sb.AppendLine($"#### {i + 1}");
    sb.AppendLine();
    sb.AppendLine("```json");
    sb.AppendLine(results[i].RouteInfo.ToString());
    sb.AppendLine("```");
}

File.WriteAllText("beep.md", sb.ToString());

string FormatTestResult(TestResult result)
{
    var testCase = result.TestCase!;

    return $"| {string.Join(" | ",
        testCase.TestApplicationScenario,
        $"{testCase.HttpMethod} {testCase.Path}",
        result.ActivityDisplayName,
        testCase.ExpectedHttpRoute,
        result.RouteInfo.HttpRouteByRawText,
        result.RouteInfo.HttpRouteByActionDescriptor,
        result.RouteInfo.HttpRouteByControllerActionAndParameters)} |";
}
