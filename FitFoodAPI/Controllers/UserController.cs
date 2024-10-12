using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
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
    public async Task<JsonResult> CreateUser([FromBody] User user)
    {
        return new JsonResult(await _service.CreateUser(user));
    }
    [HttpGet("all/{userId:guid}")]
    public async Task<JsonResult> GetByInd(Guid userId)
    {
        return new JsonResult(await _service.GetById(userId));
    }
    [HttpPut("data")]
    public async Task<JsonResult> AddData([FromBody] FitData data, [FromHeader(Name = "User")] Guid userId)
    {
        return new JsonResult(await _service.AddData(userId, data));
    }
}