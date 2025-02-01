using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Models.Fit;

public static class FitConsts
{
    public class PfcProp(double protein, double fat, double carb)
    {
        public double Protein { get; set; } = protein / INACTIVE_MULTIPLY;
        public double Fat { get; set; } = fat / INACTIVE_MULTIPLY;
        public double Carb { get; set; } = carb / INACTIVE_MULTIPLY;
    }
    
    public const UInt16 MALE_MIN_DAY_KCAL = 1800;
    public const UInt16 FEMALE_MIN_DAY_KCAL = 1200;

    public static readonly PfcProp LOSS_PFC = new PfcProp(0.4167,0.1667,0.4167);
    public static readonly PfcProp KEEP_PFC = new PfcProp(0.2, 0.2, 0.6);
    public static readonly PfcProp GAIN_PFC = new PfcProp(0.25, 0.125,0.625);
    //
    public static readonly PfcProp GAIN_PFC_G = new PfcProp(2,1,5);
    public static readonly PfcProp KEEP_PFC_G = new PfcProp(1, 1, 3);
    public static readonly PfcProp LOSS_PFC_G = new PfcProp(2, 0.8,2);
    
    public const double INACTIVE_MULTIPLY = 1.2f;
    public const double LITE_MULTIPLY = 1.375f;
    public const double MIDI_MULTIPLY = 1.55f;
    public const double HIGH_MULTIPLY = 1.725f;
    public const double SPORT_MULTIPLY = 1.9f;
    
    public const double LOSS_MULTIPLY = 0.8f;
    public const double KEEP_MULTIPLY = 1.0f;
    public const double GAIN_MULTIPLY = 1.2f;
    
    public const double HIGH_WATER_MULTIPLY = 1.1f;
    public const double SPORT_WATER_MULTIPLY = 1.15f;

    public static double USING_TYPE_AS_MULTIPLY(UsingType type_) => type_ switch
    {
        UsingType.Loss => LOSS_MULTIPLY,
        UsingType.Keep => KEEP_MULTIPLY,
        UsingType.Gain => GAIN_MULTIPLY,
        _ => KEEP_MULTIPLY
    };
}