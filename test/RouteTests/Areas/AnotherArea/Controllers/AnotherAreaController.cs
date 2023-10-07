using Microsoft.AspNetCore.Mvc;

namespace RouteTests.Controllers;

[Area("AnotherArea")]
public class AnotherAreaController : Controller
{
    public IActionResult Index() => Ok();
}
