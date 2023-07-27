using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RouteTests;

public static class RouteTestData
{
    public static IEnumerable<object[]> GetTestCases()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var input = JsonSerializer.Deserialize<RouteTestCase[]>(
            assembly.GetManifestResourceStream("RouteTests.testcases.json")!,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            });
        return GetArgumentsFromTestCaseObject(input!);
    }

    private static IEnumerable<object[]> GetArgumentsFromTestCaseObject(IEnumerable<RouteTestCase> input)
    {
        var result = new List<object[]>();

        if (input.Any(x => x.Debug))
        {
            foreach (var testCase in input.Where(x => x.Debug))
            {
                result.Add(new object[]
                {
                    testCase,
                });
            }
        }
        else
        {
            foreach (var testCase in input)
            {
                result.Add(new object[]
                {
                    testCase,
                });
            }
        }

        return result;
    }

    public class RouteTestCase
    {
        public bool Debug { get; set; }

        public TestApplicationScenario TestApplicationScenario { get; set; }

        public string? HttpMethod { get; set; }

        public string? Path { get; set; }

        public int ExpectedStatusCode { get; set; }

        public string? ExpectedHttpRoute { get; set; }
    }
}
