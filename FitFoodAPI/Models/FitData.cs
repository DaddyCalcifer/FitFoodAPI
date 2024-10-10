using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Models;

public class FitData
{
    public float Weight { get; set; }
    public float Height { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public ActivityType Activity { get; set; }
    public DateTime CreatedAt { get; }

    public FitData()
    {
        CreatedAt = DateTime.Now;
    }

    public FitData(float weight, float height, int age, Gender gender, ActivityType activity)
    {
        Weight = weight;
        Height = height;
        Age = age;
        Gender = gender;
        Activity = activity;
        CreatedAt = DateTime.Now;
    }
}