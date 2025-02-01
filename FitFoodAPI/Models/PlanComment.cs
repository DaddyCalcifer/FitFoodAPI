using System.ComponentModel.DataAnnotations.Schema;
using FitFoodAPI.Models.Fit;

namespace FitFoodAPI.Models;

public class PlanComment
{
    public Guid Id { get; set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    [ForeignKey("FitPlan")]
    public Guid PlanId { get; set; }
    public string? Text { get; set; }
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("dd.MM.yyyy");
    public string EditedAt { get; set; } = DateTime.UtcNow.ToString("dd.MM.yyyy");
    public byte Rating { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsAnonymous { get; set; } = false;
    public User? User { get; set; }
    public FitPlan? FitPlan { get; set; }
}