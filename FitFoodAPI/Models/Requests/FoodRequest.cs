namespace FitFoodAPI.Models.Requests;

public class FoodRequest
{
    public string Name { get; set; }
    public double Mass { get; set; }
    public double Kcal { get; set; }
    public double Fat { get; set; }
    public double Protein { get; set; }
    public double Carb { get; set; }
}