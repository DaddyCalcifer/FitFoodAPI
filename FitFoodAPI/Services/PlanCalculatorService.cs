using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Services;

public class PlanCalculatorService
{

    public PlanCalculatorService()
    {
    }

    public double CalculateCaloriesPerDay(FitData fitData, UsingType usingType=UsingType.Keep)
    {
        double bmr = 0;
        if (fitData.Gender == Gender.Male)
        {
            bmr = 88.36 + (13.4*fitData.Weight) + (4.8 * fitData.Height) - (5.7 * fitData.Age);
        }
        else if(fitData.Gender == Gender.Female)
        {
            bmr = 447.6 + (9.2*fitData.Weight) + (3.1 * fitData.Height) - (4.3 * fitData.Age);
        }

        switch (fitData.Activity)
        {
            case ActivityType.Inactive:
                bmr *= FitConsts.INACTIVE_MULTIPLY;
                break;
            case ActivityType.Lite:
                bmr *= FitConsts.LITE_MULTIPLY;
                break;
            case ActivityType.Midi:
                bmr *= FitConsts.MIDI_MULTIPLY;
                break;
            case ActivityType.High:
                bmr *= FitConsts.HIGH_MULTIPLY;
                break;
            case ActivityType.Sport:
                bmr *= FitConsts.SPORT_MULTIPLY;
                break;
            default:
                break;
        }

        switch (usingType)
        {
            case UsingType.Loss:
                bmr*= FitConsts.LOSS_MULTIPLY;
                break;
            case UsingType.Keep:
                bmr *= FitConsts.KEEP_MULTIPLY;
                break;
            case UsingType.Gain:
                bmr *= FitConsts.GAIN_MULTIPLY;
                break;
            default:
                break;
        }
        return bmr;
    }

    public double CalculateWaterPerDay(FitData fitData, UsingType usingType = UsingType.Keep)
    {
        double wml = 0;
        
        if (usingType == UsingType.Loss)
        {
            wml = fitData.Weight * 42;
        }
        else
        {
            wml = fitData.Weight * 35;
        }

        if (fitData.Activity == ActivityType.High)
        {
            wml *= FitConsts.HIGH_WATER_MULTIPLY;
        }
        else if (fitData.Activity == ActivityType.Sport)
        {
            wml *= FitConsts.SPORT_WATER_MULTIPLY;
        }
        return wml;
    }
    
}