using System.Net;
using FitFoodAPI.Database;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitFoodAPI.Services;

public class PlanCalculatorService
{
    private PlanDirector _planDirector;

    public async Task<FitPlan> CalculateFullPlan(Guid userId, FitData data, UsingType usingType= UsingType.Keep)
    {
        _planDirector = new PlanDirector(data, usingType);
        var builtPlan = _planDirector.BuildPlan();
        
        await using(var context = new FitEntitiesContext())
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == userId);
            
            if (user == null) return null;
            
            user.Plans.Add(builtPlan);
            context.Users.Update(user);
            
            await context.SaveChangesAsync();
        }
        
        return builtPlan;
    }

    public async Task<FitPlan> GetPlan(Guid planId, Guid userId)
    {
        await using (var context = new FitEntitiesContext())
        {
            var fitPlans = await context.Plans.FirstOrDefaultAsync(e => e.Id == planId);
            if(fitPlans.UserId != userId && !fitPlans.isPublic) return null;
            return fitPlans;
        }
    }
}