using Microsoft.AspNetCore.Mvc;

namespace RouteTests.Controllers;

[Area("MyArea")]
public class ControllerForMyAreaController : Controller
{
    public IActionResult Default() => Ok();

    public IActionResult NonDefault() => Ok();
}
