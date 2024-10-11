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

    [HttpGet("bmr")]
    public JsonResult CalculateBmr([FromBody]FitData data, [FromHeader(Name = "UsingType")]UsingType usingType=UsingType.Keep) 
        => new JsonResult(_planCalculatorService.CalculateFullPlan(data, usingType));
}