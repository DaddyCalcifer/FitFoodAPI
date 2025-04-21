namespace FitFoodAPI.Models.Sport;

public class Exercise
{
    public Guid Id { get; set; }
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan? TrainingPlan { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Sets { get; set; } = 0;
    public double Weight { get; set; } = 0.0;
    public int Reps { get; set; } = 0;
    public double RepCaloriesLoss { get; set; } = 0.0;
    public bool RepsIsSeconds { get; set; } = false;
    
    public double TotalCaloriesLoss => RepCaloriesLoss;
}