namespace FitFoodAPI.Models.Requests;

public class RePasswordRequest(Guid userId, string oldPassword, string newPassword)
{
    public Guid UserId { get; set; } = userId;
    public string OldPassword { get; set; } = oldPassword;
    public string NewPassword { get; set; } = newPassword;
}