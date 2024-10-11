using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services.Builders;

namespace FitFoodAPI.Services;

public class PlanCalculatorService()
{
    private PlanDirector _planDirector;

    public FitPlan CalculateFullPlan(FitData data, UsingType usingType= UsingType.Keep)
    {
        _planDirector = new PlanDirector(data, usingType);
        return _planDirector.BuildPlan();
    }
}