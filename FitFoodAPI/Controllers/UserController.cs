using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Requests;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/user")]
public class UserController
{
    private readonly UserService _service = new();

    [HttpGet("all")]
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
        if (result == null) return new NotFoundResult();
        return new OkObjectResult(result);
    }
    [HttpPut("data")]
    public async Task<JsonResult> AddData([FromBody] FitData data, [FromHeader(Name = "User")] Guid userId)
    {
        return new JsonResult(await _service.AddData(userId, data));
    }
}