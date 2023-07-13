using Microsoft.AspNetCore.Mvc;

namespace RouteTests.Controllers;

[ApiController]
[Route("[controller]")]
public class AttributeRouteController : ControllerBase
{
    [HttpGet]
    [HttpGet("[action]")]
    public IActionResult Get() => Ok();

    [HttpGet("[action]/{id}")]
    public IActionResult Boop(int id) => throw new NotImplementedException();
}
