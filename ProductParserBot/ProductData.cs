namespace ProductParserBot;

public class ProductData
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbohydrates { get; set; }
    public double Weight { get; set; }
    public string Ingredients { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Название: {Name}\n\tБелки: {Protein}\n\tЖиры: {Fat}\n\tУглеводы {Carbohydrates}\n\tКалории {Calories}\n";
    }
}
