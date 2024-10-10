using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/food")]
public class FoodController : ControllerBase
{
    [HttpGet("test")]
    public ActionResult<string> CtrlTest()
    {
        return Ok("It actually works!");
    }
    
    
}