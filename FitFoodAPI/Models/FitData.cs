using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Models;

public class FitData
{
    public Guid Id { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public ActivityType Activity { get; set; }
    
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User? User { get; set; }
    
    public string? UpdatedAt { get; set; }
    public string? CreatedAt { get; set; }

    public double ActivityAsMultiply() =>
        Activity switch
        {
            ActivityType.Inactive => FitConsts.INACTIVE_MULTIPLY,
            ActivityType.Lite => FitConsts.LITE_MULTIPLY,
            ActivityType.Midi => FitConsts.MIDI_MULTIPLY,
            ActivityType.High => FitConsts.HIGH_MULTIPLY,
            ActivityType.Sport => FitConsts.SPORT_MULTIPLY,
            _ => FitConsts.INACTIVE_MULTIPLY
        };

    public FitData()
    {
        CreatedAt = UpdatedAt = DateTime.UtcNow.ToString();
    }

    public FitData(float weight, float height, int age, Gender gender, ActivityType activity)
    {
        Weight = weight;
        Height = height;
        Age = age;
        Gender = gender;
        Activity = activity;
        CreatedAt = UpdatedAt = DateTime.UtcNow.ToString();
    }
}