using FitFoodAPI.Models;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculationController
{
    private readonly PlanCalculatorService _planCalculatorService = new();

    [HttpGet("bmr")]
    public JsonResult CalculateBmr([FromBody]FitData data) => new JsonResult(_planCalculatorService.CalculateFullPlan(data));
}