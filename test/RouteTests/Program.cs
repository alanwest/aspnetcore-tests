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

sb.AppendLine("| | | http.route | expected | routing type | request |");
sb.AppendLine("| - | - | - | - | - | - |");

for (var i = 0; i < results.Count; ++i)
{
    var result = results[i];
    var emoji = result.ActivityDisplayName.Equals(result.TestCase.ExpectedHttpRoute, StringComparison.InvariantCulture)
        ? ":green_heart:"
        : ":broken_heart:";
    sb.Append($"| {emoji} | [{i + 1}](#{i + 1}) ");
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

File.WriteAllText("README.md", sb.ToString());

string FormatTestResult(TestResult result)
{
    var testCase = result.TestCase!;

    return $"| {string.Join(" | ",
        result.ActivityDisplayName, // TODO: should be result.HttpRoute, but http.route is not currently added to Activity
        testCase.ExpectedHttpRoute,
        testCase.TestApplicationScenario,
        $"{testCase.HttpMethod} {testCase.Path}",
        result.ActivityDisplayName)} |";
}
