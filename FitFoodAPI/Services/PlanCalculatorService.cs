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

    public async Task<HttpStatusCode> CalculateFullPlan(Guid userId, FitData data, UsingType usingType= UsingType.Keep)
    {
        _planDirector = new PlanDirector(data, usingType);
        var builtPlan = _planDirector.BuildPlan();
        
        await using(var context = new FitEntitiesContext())
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == userId);
            
            if (user == null) return HttpStatusCode.NotFound;
            
            user.Plans.Add(builtPlan);
            context.Users.Update(user);
            
            await context.SaveChangesAsync();
        }
        
        return HttpStatusCode.OK;
    }

    public FitPlan GetPlan(Guid planId)
    {
        using (var context = new FitEntitiesContext())
        {
            var fitPlans = context.Plans.FirstOrDefault(e => e.Id == planId);
            return fitPlans;
        }
    }
}