using System.Net;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/fit")]
public class PlanController
{
    private readonly PlanCalculatorService _planCalculatorService = new();
    private readonly CommentService _commentService = new();

    [HttpPost("default")]
    public async Task<IActionResult> CalculateBmr([FromBody]FitData data,
        [FromHeader(Name = "User")]Guid userId,
        [FromHeader(Name = "UsingType")]UsingType usingType=UsingType.Keep)
    {
        var plan = await _planCalculatorService.CalculateFullPlan(userId, data, usingType);
        plan.User = null;
        
        Console.WriteLine($"Plan {plan.Id} has been built for user {userId}");
        return new OkObjectResult(plan);
    }

    [HttpGet("{planId:guid}")]
    public async Task<IActionResult> GetById(Guid planId, [FromHeader(Name = "User")]Guid userId)
    {
        var plann = await _planCalculatorService.GetPlan(planId, userId);
        if(plann == null)
            return new NotFoundResult();
        else return new OkObjectResult(plann);
    }
    [HttpGet("{planId:guid}/comments")]
    public async Task<IActionResult> GetCommentsById(Guid planId, [FromHeader(Name = "User")]Guid userId)
    {
        var plann = await _planCalculatorService.GetPlanComments(planId, userId);
        if(plann == null)
            return new NoContentResult();
        else return new OkObjectResult(plann);
    }
    [HttpPost("{planId:guid}/comments/add")]
    public async Task<IActionResult> AddCommentsById([FromBody]PlanComment comment)
    {
        var plann = await _commentService.AddComment(comment);
        if(plann == null)
            return new NoContentResult();
        else return new OkObjectResult(plann);
    }
    
    [HttpGet("{planId:guid}/rating")]
    public async Task<IActionResult> GetRatingById(Guid planId, [FromHeader(Name = "User")]Guid userId)
    {
        var plann = await _planCalculatorService.GetPlanRating(planId);
        if(plann == null)
            return new NoContentResult();
        return new OkObjectResult(plann);
    }
}