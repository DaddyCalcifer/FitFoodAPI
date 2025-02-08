namespace FitFoodAPI.Models.Sport;

public class ExerciseProgress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ExerciseId { get; set; }
    public Guid TrainingId { get; set; }
    public Exercise? Exercise { get; set; } = null;
    public Training? Training { get; set; } = null;
    public List<Set> Sets { get; set; } = [];
}