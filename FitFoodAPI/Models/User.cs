namespace FitFoodAPI.Models;

public class User(string username, string password, string email = "")
{
    public Guid Id { get; set; }
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    public string Email { get; set; } = email;
    
    public ICollection<FitPlan> Plans { get; set; } = new List<FitPlan>();
    public ICollection<FitData> Datas { get; set; } = new List<FitData>();
}