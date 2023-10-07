using Microsoft.AspNetCore.Mvc;

namespace RouteTests.Controllers;

[Area("MyArea")]
public class MyAreaController : Controller
{
    public IActionResult Default() => Ok();
}
