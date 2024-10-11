using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Services.Builders;

public class PlanDirector(FitData data, UsingType usingType)
{
    IPlanBuilder _planBuilder = data.Gender switch
    {
        Gender.Male => new MalePlanBuilder(data, usingType),
        Gender.Female => new FemalePlanBuilder(data, usingType),
        _ => throw new ArgumentException("Invalid gender")
    };

    public FitPlan BuildPlan()
    {
        return _planBuilder
            .bDurationInDays()
            .bDayWater()
            .bCarb()
            .bProtein()
            .bFat()
            .build();
    }
}