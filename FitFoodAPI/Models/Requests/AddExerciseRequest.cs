namespace FitFoodAPI.Models.Requests;

public class AddExerciseRequest
{
    public string name { get; set; }
    public string description { get; set; }
    public int sets { get; set; } = 0;
    public double weight { get; set; } = 0.0;
    public int reps { get; set; } = 0;
    public double repCaloriesLoss { get; set; } = 0.0;
    public bool repsIsSeconds { get; set; } = false;
}