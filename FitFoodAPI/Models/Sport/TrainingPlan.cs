namespace FitFoodAPI.Models.Sport;

public class TrainingPlan
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Exercise> Exercises { get; set; } = [];
    
    public string Date { get; set; } = DateTime.UtcNow.ToString("dd.MM.yyyy");
    public Guid UserId { get; set; }
    public User? User { get; set; } = null;
    public bool isDeleted { get; set; } = false;
    
    public double CaloriesLoss => Exercises.Sum(ex => ex.TotalCaloriesLoss);
}