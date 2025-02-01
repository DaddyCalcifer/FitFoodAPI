using System.Security.Claims;
using System.Text.RegularExpressions;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Fit;
using FitFoodAPI.Models.Requests;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/user")]
public partial class UserController : ControllerBase
{
    private readonly UserService _service = new();

    [HttpGet("all")]
    [Authorize]
    public async Task<JsonResult> GetAll()
    {
        return new JsonResult(await _service.GetAll());
    }
    [HttpPost("create")]
    public async Task<JsonResult> CreateUser([FromBody] User user)
    {
        if(user.Username.Length < 3 || user.Username.Length > 20) 
            return new JsonResult(new { message = "Ошибка регистрации: логин должен состоять из 3-20 символов!" }) 
                { StatusCode = StatusCodes.Status400BadRequest };
        if(user.Password.Length < 5 || user.Password.Length > 20) 
            return new JsonResult(new { message = "Ошибка регистрации: пароль должен состоять из 5-20 символов!" }) 
                { StatusCode = StatusCodes.Status400BadRequest };
        if (!EmailRegex().IsMatch(user.Email))
            return new JsonResult(new { message = "Ошибка регистрации: неверный формат email!" }) 
                { StatusCode = StatusCodes.Status400BadRequest };
        
        var result = await _service.CreateUser(user);
        return result != null ? new JsonResult(new { message = "Пользователь успешно зарегестрирован" }) { StatusCode = StatusCodes.Status201Created } 
            : new JsonResult(new { message = "При регистрации возникла ошибка, логин или e-mail занят." }) { StatusCode = StatusCodes.Status400BadRequest };
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
    public async Task<JsonResult> Authorize([FromBody] AuthRequest request)
    {
        if(request.Login.Length < 2 || request.Login.Length > 20) 
            return new JsonResult(new { value = "", message = "Ошибка авторизации: проверьте данные!" }) { StatusCode = 401 };
        if(request.Password.Length < 5) 
            return new JsonResult(new { value = "", message = "Ошибка авторизации: проверьте данные!" }) { StatusCode = 401 };
        var result = await _service.Authorize(request);
        return result != "" ? new JsonResult(new { value = result, message = "Успешно авторизован!" }) { StatusCode = 200 } 
            : new JsonResult(new { value = "", message = "Ошибка авторизации: проверьте данные!" }) { StatusCode = 401 };
    }
    [HttpPut("data")]
    [Authorize]
    public async Task<JsonResult> AddData([FromBody] FitData data)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }

        await _service.AddData(userId, data);
        return new JsonResult(new {message = "Data addiction success!"}) { StatusCode = StatusCodes.Status201Created };
    }
    [HttpPatch("settings")]
    [Authorize]
    public async Task<JsonResult> ChangeSettings([FromBody] User data)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        data.Id = userId;

        await _service.UpdateUserData(data);
        return new JsonResult(new {message = "Data was updated!"}) { StatusCode = StatusCodes.Status205ResetContent };
    }
    [HttpPatch("repassword")]
    [Authorize]
    public async Task<JsonResult> ChangePassword([FromBody] RePasswordRequest data)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        data.UserId = userId;

        await _service.UpdatePassword(data);
        return new JsonResult(new {message = "Data was updated!"}) { StatusCode = StatusCodes.Status205ResetContent };
    }

    [HttpGet]
    [Authorize]
    public async Task<JsonResult> Self()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var user = await _service.GetById(userId);
        
        return user == null ? 
            new JsonResult(new {message = "Not found!" }) { StatusCode = StatusCodes.Status404NotFound } : 
            new JsonResult(user) { StatusCode = StatusCodes.Status200OK };
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}