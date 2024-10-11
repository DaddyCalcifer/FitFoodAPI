namespace FitFoodAPI.Models;

public static class FitConsts
{
    public const UInt16 MALE_MIN_DAY_KCAL = 1800;
    public const UInt16 FEMALE_MIN_DAY_KCAL = 1200;
    
    public const double MALE_PROTEIN_PART = 0.25;
    public const double MALE_FAT_PART = 0.15;
    public const double MALE_CARB_PART = 0.6;
    //
    public const double FEMALE_PROTEIN_PART = 0.25;
    public const double FEMALE_FAT_PART = 0.25;
    public const double FEMALE_CARB_PART = 0.5;
    
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
}