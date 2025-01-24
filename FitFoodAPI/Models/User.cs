using System.ComponentModel.DataAnnotations;

namespace FitFoodAPI.Models;

public class User(string username, string password, string email = "")
{
    public Guid Id { get; set; }
    [MaxLength(25)]
    public string Username { get; set; } = username;
    
    [MaxLength(512)]
    public string Password { get; set; } = password;
    [MaxLength(25)]
    public string Email { get; set; } = email;
    
    public ICollection<FitPlan> Plans { get; set; } = new List<FitPlan>();
    public ICollection<FitData> Datas { get; set; } = new List<FitData>();
    public ICollection<FeedAct> FeedActs { get; set; } = new List<FeedAct>();
}