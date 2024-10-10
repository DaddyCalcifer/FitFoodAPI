using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    public TestController()
    {
        
    }
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("test success!");
    }
}