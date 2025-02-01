namespace FitFoodAPI.Models.Sport;

public class Training
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User? User { get; set; } = null;
}