namespace FitFoodAPI.Models;

public class PlanRating(double rating, int count)
{
    public double Rating { get; set; }
    public int CommentsCount { get; set; }
}