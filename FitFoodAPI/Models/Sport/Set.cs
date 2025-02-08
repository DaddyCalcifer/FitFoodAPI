namespace FitFoodAPI.Models.Sport;

public class Set
{
    public Guid Id { get; set; }
    public byte SetNumber { get; set; } = 1;
    public Guid ExerciseProgressId { get; set; }
    public ExerciseProgress? ExerciseProgress { get; set; } = null;
    public double Reps { get; set; } = 0.0;
    public double Weight { get; set; } = 0.0;
    public bool isCompleted { get; set; } = false;

    public Set()
    {
    }
    public Set(byte setNumber, double reps, double weight = 0)
    {
        this.SetNumber = setNumber;
        this.Reps = reps;
        this.Weight = weight;
    }

    public bool RepsIsTime
    {
        get
        {
            var exercise = this.ExerciseProgress?.Exercise;
            return exercise != null ? exercise.RepsIsSeconds : false;
        }
    }
}