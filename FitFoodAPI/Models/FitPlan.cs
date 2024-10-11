namespace FitFoodAPI.Models;

public class FitPlan
{
    public double DayKcal { get; set; }
    public int DurationInDays { get; set; }
    public double WaterMl { get; set; }
    
    public double Fat { get; set; }
    public double Protein { get; set; }
    public double Carb { get; set; }
    public double BreakfastKcal => Math.Round(DayKcal*0.3);
    public double LunchKcal => Math.Round(DayKcal*0.4);
    public double DinnerKcal => Math.Round(DayKcal*0.3);
    
}