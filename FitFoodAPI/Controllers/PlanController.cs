using System.Net;
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
[Route("api/fit")]
public class PlanController : ControllerBase
{
    private readonly PlanCalculatorService _planCalculatorService = new();
    private readonly CommentService _commentService = new();

    [HttpPost("default")]
    [Authorize]
    public async Task<JsonResult> CalculateBmr(
        [FromBody]GeneratePlanRequest request)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var data = await _planCalculatorService.GetData(request.FitDataId, userId);
        if(data == null) return new JsonResult(new { message = "Invalid data!" }) { StatusCode = StatusCodes.Status404NotFound };
        
        var plan = await _planCalculatorService.CalculateFullPlan(userId, data, request.UsingType);
        plan.User = null;
        
        Console.WriteLine($"Plan {plan.Id} has been built for user {userId}");
        return new JsonResult(plan){ StatusCode = StatusCodes.Status201Created };
    }
    [HttpPatch("{planId:guid}/recalculate")]
    [Authorize]
    public async Task<JsonResult> RecalculateBmr(
        [FromBody]GeneratePlanRequest request, Guid planId)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var data = await _planCalculatorService.GetData(request.FitDataId, userId);
        if(data == null) return new JsonResult(new { message = "Invalid data!" }) { StatusCode = StatusCodes.Status404NotFound };

        var planTest = await _planCalculatorService.GetPlan(planId, userId);
        if(planTest == null || planTest.UserId != userId) return new JsonResult(new { message = "Invalid data!" }) { StatusCode = StatusCodes.Status401Unauthorized };
        
        var plan = await _planCalculatorService.ReCalculatePlan(planId, data, request.UsingType);
        plan.User = null;
        
        Console.WriteLine($"Plan {plan.Id} has been rebuilt for user {userId}");
        return new JsonResult(plan){ StatusCode = StatusCodes.Status201Created };
    }
    [HttpDelete("{planId:guid}/delete")]
    [Authorize]
    public async Task<JsonResult> DeletePlan(Guid planId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await _planCalculatorService.DeletePlan(planId);
        
        return result ?
            new JsonResult(new { message = "Something is wrong! Plan was not deleted" }) { StatusCode = StatusCodes.Status400BadRequest } :
            new JsonResult(new { message = "Plan was deleted!" }) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpPut("{planId:guid}/public:{isPublic:bool}")]
    [Authorize]
    public async Task<JsonResult> PlanVisibility(Guid planId, bool isPublic)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await _planCalculatorService.ChangePlanVisibility(planId, isPublic);
        
        return result ?
            new JsonResult(new { message = "Something is wrong! Visibility was not changed" }) { StatusCode = StatusCodes.Status400BadRequest } :
            new JsonResult(new { message = "Plan visibility was changed!" }) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("{planId:guid}")]
    [Authorize]
    public async Task<JsonResult> GetById(Guid planId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plan = await _planCalculatorService.GetPlan(planId, userId);
        
        return plan == null ? 
            new JsonResult(new { message = "Plan not found!" }) { StatusCode = 404 } : 
            new JsonResult(plan) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpGet("{planId:guid}/comments")]
    [Authorize]
    public async Task<JsonResult> GetCommentsById(Guid planId)
    {
        // Извлекаем userId из токена
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        
        var plann = await _planCalculatorService.GetPlanComments(planId, userId);
        return plann == null ? 
            new JsonResult(new { message = "There are no comments!" }) { StatusCode = StatusCodes.Status204NoContent } : 
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
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        comment.UserId = userId;
        
        var plann = await _commentService.AddComment(comment);
        return plann == 0 ? 
            new JsonResult(new {message = "Comment not added!"}) { StatusCode = StatusCodes.Status400BadRequest } : 
            new JsonResult(plann){StatusCode = StatusCodes.Status201Created};
    }
    
    [HttpGet("{planId:guid}/rating")]
    [Authorize]
    public async Task<JsonResult> GetRatingById(Guid planId)
    {
        var planRating = await _planCalculatorService.GetPlanRating(planId);
        return planRating == null ? 
            new JsonResult(new { message = "There is no rating found!"}){ StatusCode = StatusCodes.Status404NotFound } : 
            new JsonResult(planRating){ StatusCode = StatusCodes.Status200OK };
    }
    [HttpDelete("{planId:guid}/comments/{commentId:guid}/delete")]
    [Authorize]
    public async Task<JsonResult> DeleteComment(Guid planId, Guid commentId)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await _commentService.DeleteComment(commentId, userId);
        
        return result ?
            new JsonResult(new { message = "Something is wrong! Comment was not deleted" }) { StatusCode = StatusCodes.Status400BadRequest } :
            new JsonResult(new { message = "Comment was deleted!" }) { StatusCode = StatusCodes.Status200OK };
    }
    [HttpPatch("{planId:guid}/comments/{commentId:guid}/edit")]
    [Authorize]
    public async Task<JsonResult> EditComment(Guid planId, Guid commentId, [FromBody]EditCommentRequest request)
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return new JsonResult(new { message = "Token is incorrect!" }) { StatusCode = 401 };
        }
        var result = await _commentService.EditComment(commentId, userId, request);
        
        return result ?
            new JsonResult(new { message = "Something is wrong! Comment was not edited" }) { StatusCode = StatusCodes.Status400BadRequest } :
            new JsonResult(new { message = "Comment was edited." }) { StatusCode = StatusCodes.Status200OK };
    }
}