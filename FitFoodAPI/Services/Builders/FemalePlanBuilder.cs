using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Fit;

namespace FitFoodAPI.Services.Builders;

public class FemalePlanBuilder : IPlanBuilder
{
    private readonly FitPlan _plan;
    private readonly FitData _data;
    private readonly UsingType _usingType;

    public FemalePlanBuilder(FitData data, UsingType usingType = UsingType.Keep)
    {
        this._data = data;
        this._usingType = usingType;
        this._plan = new FitPlan();
        bDayKcal();
    }
    
    public void bDayKcal()
    {
        var bmr = 447.6 + (9.2*_data.Weight) + (3.1 * _data.Height) - (4.3 * _data.Age);
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
        if(bmr < FitConsts.FEMALE_MIN_DAY_KCAL) bmr = FitConsts.FEMALE_MIN_DAY_KCAL;
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
        if(wml < 2000) wml = 2000;
        _plan.WaterMl = Math.Round(wml);
        return this;
    }

    public IPlanBuilder bDurationInDays()
    {
        return this;
    }

    public IPlanBuilder bProtein()
    {
        _plan.Protein_kcal = Math.Round(_plan.DayKcal *
                             _usingType switch
        {
            UsingType.Loss => FitConsts.LOSS_PFC.Protein,
            UsingType.Keep => FitConsts.KEEP_PFC.Protein,
            UsingType.Gain => FitConsts.GAIN_PFC.Protein,
            _ => FitConsts.KEEP_PFC.Protein
        });
        _plan.Protein_g = Math.Round(_data.Weight 
                                     * FitConsts.USING_TYPE_AS_MULTIPLY(_usingType) 
                                     * _data.ActivityAsMultiply()
                                     * _usingType switch
        {
            UsingType.Loss => FitConsts.LOSS_PFC_G.Protein,
            UsingType.Keep => FitConsts.KEEP_PFC_G.Protein,
            UsingType.Gain => FitConsts.GAIN_PFC_G.Protein,
            _ => FitConsts.KEEP_PFC_G.Protein
        });
        return this;
    }

    public IPlanBuilder bCarb()
    {
        _plan.Carb_kcal = Math.Round(_plan.DayKcal *
                                     _usingType switch
                                     {
                                         UsingType.Loss => FitConsts.LOSS_PFC.Carb,
                                         UsingType.Keep => FitConsts.KEEP_PFC.Carb,
                                         UsingType.Gain => FitConsts.GAIN_PFC.Carb,
                                         _ => FitConsts.KEEP_PFC.Carb
                                     });
        _plan.Carb_g = Math.Round(_data.Weight 
                                  * FitConsts.USING_TYPE_AS_MULTIPLY(_usingType)
                                  * _data.ActivityAsMultiply()
                                  *  _usingType switch
        {
            UsingType.Loss => FitConsts.LOSS_PFC_G.Carb,
            UsingType.Keep => FitConsts.KEEP_PFC_G.Carb,
            UsingType.Gain => FitConsts.GAIN_PFC_G.Carb,
            _ => FitConsts.KEEP_PFC_G.Carb
        });
        return this;
    }

    public IPlanBuilder bFat()
    {
        _plan.Fat_kcal = Math.Round(_plan.DayKcal *
                                    _usingType switch
                                    {
                                        UsingType.Loss => FitConsts.LOSS_PFC.Fat,
                                        UsingType.Keep => FitConsts.KEEP_PFC.Fat,
                                        UsingType.Gain => FitConsts.GAIN_PFC.Fat,
                                        _ => FitConsts.KEEP_PFC.Fat
                                    });
        _plan.Fat_g = Math.Round(_data.Weight 
                                 * FitConsts.USING_TYPE_AS_MULTIPLY(_usingType) 
                                 * _data.ActivityAsMultiply()
                                 * _usingType switch
        {
            UsingType.Loss => FitConsts.LOSS_PFC_G.Fat,
            UsingType.Keep => FitConsts.KEEP_PFC_G.Fat,
            UsingType.Gain => FitConsts.GAIN_PFC_G.Fat,
            _ => FitConsts.KEEP_PFC_G.Fat
        });
        return this;
    }

    public FitPlan build()
    {
        _plan.UserId = _data.UserId;
        return _plan;
    }
}