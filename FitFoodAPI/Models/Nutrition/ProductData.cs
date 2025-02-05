namespace FitFoodAPI.Models.Nutrition;

public class ProductData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public double Calories { get; set; } = 0.0;
    public double Protein { get; set; } = 0.0;
    public double Fat { get; set; } = 0.0;
    public double Carbohydrates { get; set; } = 0.0;
    public double Weight { get; set; } = 0.0;
    public string Ingredients { get; set; } = string.Empty;
    
}