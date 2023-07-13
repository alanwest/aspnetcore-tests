using Microsoft.AspNetCore.Mvc;

namespace RouteTests.Controllers;

public class ConventionalRouteController : Controller
{
    public IActionResult Default() => Ok();

    public IActionResult ActionWithParameter(int id) => Ok();

    public IActionResult ActionWithStringParameter(string id, int num) => Ok();
}
