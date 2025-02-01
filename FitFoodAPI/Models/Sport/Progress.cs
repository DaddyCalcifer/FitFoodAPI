namespace FitFoodAPI.Models.Sport;

public class Progress
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public string Date { get; set; } = DateTime.UtcNow.ToString("dd.MM.yyyy");
    public int Sets { get; set; }
    public double Weight { get; set; }
    public int Reps { get; set; }
    public bool Completed { get; set; }
}