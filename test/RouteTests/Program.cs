using RouteTests;

var testCases = RoutingTests.TestData;

var sb = new System.Text.StringBuilder();

var tuples = new List<(string summary, string details)>();

foreach (var item in testCases)
{
    using var t = new RoutingTests(null);
    var testCase = item[0] as RouteTestData.RouteTestCase;
    var result = await t.ExampleTest(testCase!);
    tuples.Add(result);
}

Console.WriteLine("| | Scenario | Expected | Actual |");
Console.WriteLine("| - | - | - | - |");

for (var i = 0; i < tuples.Count; ++i)
{
    Console.Write($"| [{i + 1}](#{i + 1}) ");
    Console.WriteLine(tuples[i].summary);
}

for (var i = 0; i < tuples.Count; ++i)
{
    Console.WriteLine();
    Console.WriteLine($"#### {i + 1}");
    Console.WriteLine();
    Console.WriteLine("```json");
    Console.WriteLine(tuples[i].details);
    Console.WriteLine("```");
}
