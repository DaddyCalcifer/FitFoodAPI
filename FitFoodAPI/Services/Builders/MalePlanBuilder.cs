using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;

namespace FitFoodAPI.Services.Builders;

public class MalePlanBuilder : IPlanBuilder
{
    private readonly FitPlan _plan;
    private readonly FitData _data;
    private readonly UsingType _usingType;

    public MalePlanBuilder(FitData data, UsingType usingType = UsingType.Keep)
    {
        this._data = data;
        this._usingType = usingType;
        this._plan = new FitPlan();
        bDayKcal();
    }
    
    public void bDayKcal()
    {
        var bmr = 88.36 + (13.4*_data.Weight) + (4.8 * _data.Height) - (5.7 * _data.Age);
        switch (_data.Activity)
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

        switch (_usingType)
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
        _plan.DayKcal = Math.Round(bmr);
    }

    public IPlanBuilder bDayWater()
    {
        double wml = 0;
        
        if (_usingType == UsingType.Loss)
        {
            wml = _data.Weight * 40;
        }
        else
        {
            wml = _data.Weight * 30;
        }

        switch (_data.Activity)
        {
            case ActivityType.High:
                wml *= FitConsts.HIGH_WATER_MULTIPLY;
                break;
            case ActivityType.Sport:
                wml *= FitConsts.SPORT_WATER_MULTIPLY;
                break;
        }
        _plan.WaterMl = Math.Round(wml);
        return this;
    }

    public IPlanBuilder bDurationInDays()
    {
        return this;
    }

    public IPlanBuilder bProtein()
    {
        _plan.Protein = Math.Round(_plan.DayKcal * FitConsts.MALE_PROTEIN_PART);
        return this;
    }

    public IPlanBuilder bCarb()
    {
        _plan.Carb = Math.Round(_plan.DayKcal * FitConsts.MALE_CARB_PART);
        return this;
    }

    public IPlanBuilder bFat()
    {
        _plan.Fat = Math.Round(_plan.DayKcal * FitConsts.MALE_FAT_PART);
        return this;
    }

    public FitPlan build() => _plan;
}