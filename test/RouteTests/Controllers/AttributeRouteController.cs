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
    public IActionResult Get(int id) => Ok();

    [HttpGet("{id}/[action]")]
    public IActionResult GetWithActionNameInDifferentSpotInTemplate(int id) => Ok();
}
