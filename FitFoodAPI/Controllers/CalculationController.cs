using FitFoodAPI.Models;
using FitFoodAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitFoodAPI.Controllers;

[ApiController]
[Route("api/calculator")]
public class CalculationController
{
    private readonly PlanCalculatorService _planCalculatorService;

    public CalculationController()
    {
        _planCalculatorService = new PlanCalculatorService();
    }

    [HttpGet("bmr")]
    public JsonResult CalculateBMR([FromBody]FitData data)
    {
        FitPlan plan = new FitPlan();
        plan.DayKcal = Math.Round(_planCalculatorService.CalculateCaloriesPerDay(data));
        plan.DurationInDays = 10;
        plan.WaterMl = Math.Round(_planCalculatorService.CalculateWaterPerDay(data));
        return new JsonResult(plan);
    }
}