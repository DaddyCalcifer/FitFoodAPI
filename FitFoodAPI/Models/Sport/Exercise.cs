namespace FitFoodAPI.Models.Sport;

public class Exercise
{
    public Guid Id { get; set; }
    public Guid TrainingId { get; set; }
    public Training? Training { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Sets { get; set; } = 0;
    public double Weight { get; set; } = 0.0;
    public int Reps { get; set; } = 0;
    public bool RepsIsTime { get; set; } = false;
}