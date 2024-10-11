namespace FitFoodAPI.Models;

public class FitPlan
{
    public double DayKcal { get; set; }
    public int DurationInDays { get; set; }
    public double WaterMl { get; set; }
    
    public double Fat_kcal { get; set; }
    public double Protein_kcal { get; set; }
    public double Carb_kcal { get; set; }
    //
    public double Fat_g { get; set; }
    public double Protein_g { get; set; }
    public double Carb_g { get; set; }
    
    public Guid UserId { get; set; }
    
    public double BreakfastKcal => Math.Round(DayKcal*0.3);
    public double LunchKcal => Math.Round(DayKcal*0.4);
    public double DinnerKcal => Math.Round(DayKcal*0.3);
    
}