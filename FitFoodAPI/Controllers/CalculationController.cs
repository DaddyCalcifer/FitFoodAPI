using System.Net;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/fit")]
public class CalculationController
{
    private readonly PlanCalculatorService _planCalculatorService = new();
    private readonly UserService _userService = new();

    [HttpPost("default")]
    public async Task<IActionResult> CalculateBmr([FromBody]FitData data,
        [FromHeader(Name = "User")]Guid userId,
        [FromHeader(Name = "UsingType")]UsingType usingType=UsingType.Keep)
    {
        var plan = await _planCalculatorService.CalculateFullPlan(userId, data, usingType);
        
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
}