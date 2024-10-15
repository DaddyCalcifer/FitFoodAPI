namespace FitFoodAPI.Models.Requests;

public class AuthRequest(string login, string password)
{
    public string Login { get; set; } = login;
    public string Password { get; set; } = password;
}