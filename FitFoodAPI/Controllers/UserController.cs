using System.Security.Claims;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Requests;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _service = new();

    [HttpGet("all")]
    [Authorize]
    public async Task<JsonResult> GetAll()
    {
        return new JsonResult(await _service.GetAll());
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        return new OkObjectResult(await _service.CreateUser(user));
    }
    [HttpGet("all/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetByInd(Guid userId)
    {
        var user_ = await _service.GetById(userId);
        if (user_ == null) return new NotFoundResult();
        return new OkObjectResult(user_);
    }
    [HttpPatch("authorize")]
    public async Task<IActionResult> Authorize([FromBody] AuthRequest request)
    {
        var result = await _service.Authorize(request);
        if (result == null) return new UnauthorizedResult();
        return new OkObjectResult(result);
    }
    [HttpPut("data")]
    [Authorize]
    public async Task<JsonResult> AddData([FromBody] FitData data)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { error = "Token is incorrect!" }) { StatusCode = 401 };
        }

        await _service.AddData(userId, data);
        return new JsonResult(new {message = "Data addiction success!"}) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpGet]
    [Authorize]
    public async Task<JsonResult> Self()
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { error = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var user = await _service.GetById(userId);
        
        return user == null ? 
            new JsonResult(new {error = "Not found!" }) { StatusCode = StatusCodes.Status404NotFound } : 
            new JsonResult(new { user = user }) { StatusCode = StatusCodes.Status200OK };
    }
}