using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculationController
{
    private readonly PlanCalculatorService _planCalculatorService = new();
    private readonly UserService _userService = new();

    [HttpPost("full")]
    public async Task<JsonResult> CalculateBmr([FromBody]FitData data,
        [FromHeader(Name = "User")]Guid userId,
        [FromHeader(Name = "UsingType")]UsingType usingType=UsingType.Keep)
    {
        var planId = await _planCalculatorService.CalculateFullPlan(userId, data, usingType);
        
        Console.WriteLine($"Plan {planId} has been built for user {userId}");
        return new JsonResult(planId);
    }
}