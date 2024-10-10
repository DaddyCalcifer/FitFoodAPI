namespace FitFoodAPI.Models;

public class ProductData
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Kj { get; set; }
    public float Kcal { get; set; }
    public float Fat  { get; set; }
    public float Carb  { get; set; }
    public float Protein { get; set; }

    public ProductData()
    {
        Name = string.Empty;
    }
}