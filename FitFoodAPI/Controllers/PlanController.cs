using System.Net;
using System.Security.Claims;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/fit")]
public class PlanController : ControllerBase
{
    private readonly PlanCalculatorService _planCalculatorService = new();
    private readonly CommentService _commentService = new();

    [HttpPost("default")]
    [Authorize]
    public async Task<JsonResult> CalculateBmr(
        [FromBody]FitData data,
        [FromHeader(Name = "UsingType")]UsingType usingType=UsingType.Keep)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { error = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plan = await _planCalculatorService.CalculateFullPlan(userId, data, usingType);
        plan.User = null;
        
        Console.WriteLine($"Plan {plan.Id} has been built for user {userId}");
        return new JsonResult(plan){ StatusCode = StatusCodes.Status201Created };
    }

    [HttpGet("{planId:guid}")]
    [Authorize]
    public async Task<JsonResult> GetById(Guid planId)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { error = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plann = await _planCalculatorService.GetPlan(planId, userId);
        return plann == null ? 
            new JsonResult(new { error = "Plan not found!" }) { StatusCode = 404 } : 
            new JsonResult(plann) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("{planId:guid}/comments")]
    [Authorize]
    public async Task<JsonResult> GetCommentsById(Guid planId)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { error = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plann = await _planCalculatorService.GetPlanComments(planId, userId);
        return plann == null ? 
            new JsonResult(new { error = "There are no comments!" }) { StatusCode = StatusCodes.Status204NoContent } : 
            new JsonResult(plann){StatusCode = StatusCodes.Status200OK};
    }
    [HttpPost("{planId:guid}/comments/add")]
    [Authorize]
    public async Task<JsonResult> AddCommentsById([FromBody]PlanComment comment)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { error = "Token is incorrect!" }) { StatusCode = 401 };
        }
        comment.UserId = userId;
        
        var plann = await _commentService.AddComment(comment);
        return plann == 0 ? 
            new JsonResult(new {error = "Comment not added!"}) { StatusCode = StatusCodes.Status400BadRequest } : 
            new JsonResult(plann){StatusCode = StatusCodes.Status201Created};
    }
    
    [HttpGet("{planId:guid}/rating")]
    [Authorize]
    public async Task<JsonResult> GetRatingById(Guid planId)
    {
        var planRating = await _planCalculatorService.GetPlanRating(planId);
        return planRating == null ? 
            new JsonResult(new { error = "There is no rating found!"}){ StatusCode = StatusCodes.Status404NotFound } : 
            new JsonResult(planRating){ StatusCode = StatusCodes.Status200OK };
    }
}