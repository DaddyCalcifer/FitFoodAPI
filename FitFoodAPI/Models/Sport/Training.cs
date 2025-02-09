namespace FitFoodAPI.Models.Sport;

public class Training
{
    public Guid Id { get; set; }
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan? TrainingPlan { get; set; }
    public List<ExerciseProgress> Exercises { get; set; } = new();
    
    public string Date { get; set; } = DateTime.UtcNow.ToString("dd.MM.yyyy");
    public Guid UserId { get; set; }
    public User? User { get; set; } = null;

    public double CaloriesBurnt
    {
        get
        {
            //return Exercises.Sum(ex => ex.Exercise!.RepCaloriesLoss * (ex.Exercise.Sets * ex.Exercise.Reps));
            return 0;
        }
    }
}