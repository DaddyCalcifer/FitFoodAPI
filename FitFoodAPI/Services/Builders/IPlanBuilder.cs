using FitFoodAPI.Models;

namespace FitFoodAPI.Services.Builders;

public interface IPlanBuilder
{
    protected abstract void bDayKcal();
    public abstract IPlanBuilder bDayWater();
    public abstract IPlanBuilder bDurationInDays();
    public abstract IPlanBuilder bProtein();
    public abstract IPlanBuilder bCarb();
    public abstract IPlanBuilder bFat();
    public abstract FitPlan build(); 
}