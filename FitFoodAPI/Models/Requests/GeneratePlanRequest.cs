using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Models.Requests;

public class GeneratePlanRequest
{
    public Guid FitDataId { get; set; }
    public UsingType UsingType { get; set; } = UsingType.Keep;
}