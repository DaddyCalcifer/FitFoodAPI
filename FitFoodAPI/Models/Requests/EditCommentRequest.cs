namespace FitFoodAPI.Models.Requests;

public class EditCommentRequest
{
    public string Text { get; set; }
    public byte Rating { get; set; }
}