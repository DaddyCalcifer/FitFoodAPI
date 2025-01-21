using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Models;

public class FeedAct
{
    public FeedAct(Guid userId, 
        string date, 
        FeedType feedType, 
        string name, 
        double mass, 
        double kcal100, 
        double fat100, 
        double protein100, 
        double carb100)
    {
        UserId = userId;
        Date = date;
        FeedType = feedType;
        Name = name;
        Mass = mass;
        Kcal100 = kcal100;
        Fat100 = fat100;
        Protein100 = protein100;
        Carb100 = carb100;
    }

    public FeedAct()
    {
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public string Date { get; set; }
    public FeedType FeedType { get; set; }
    public string Name { get; set; }
    public double Mass { get; set; }
    public double Kcal100 { get; set; }
    public double Fat100 { get; set; }
    public double Protein100 { get; set; }
    public double Carb100 { get; set; }
    
    public double Fat => Math.Round(Fat100*(Mass/100));
    public double Protein => Math.Round(Protein100*(Mass/100));
    public double Carb => Math.Round(Carb100*(Mass/100));
    public double Kcal => Math.Round(Kcal100*(Mass/100));
}