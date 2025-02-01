using System.ComponentModel.DataAnnotations.Schema;

namespace FitFoodAPI.Models.Fit;

public class FitPlan
{
    public Guid Id { get; set; }
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
    
    public bool isPublic { get; set; } = false;
    public bool isDeleted { get; set; } = false;
    
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public void Reset(FitPlan plan)
    {
        this.DayKcal = plan.DayKcal;
        this.DurationInDays = plan.DurationInDays;
        this.WaterMl = plan.WaterMl;
        this.Carb_g = plan.Carb_g;
        this.Protein_g = plan.Protein_g;
        this.Fat_g = plan.Fat_g;
        this.Carb_kcal = plan.Carb_kcal;
        this.Protein_kcal = plan.Protein_kcal;
        this.Fat_kcal = plan.Fat_kcal;
    }

    public ICollection<PlanComment> Comments { get; set; } = new List<PlanComment>();
    
    public double BreakfastKcal => Math.Round(DayKcal*0.275);
    public double LunchKcal => Math.Round(DayKcal*0.35);
    public double DinnerKcal => Math.Round(DayKcal*0.275);
    public double OtherKcal => Math.Round(DayKcal*0.10);
    
}