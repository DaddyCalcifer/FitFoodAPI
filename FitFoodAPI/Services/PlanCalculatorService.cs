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
    public async Task<FitPlan> CalculateFullPlan(Guid userId, FitData data, UsingType usingType= UsingType.Keep)
    {
        var planDirector = new PlanDirector(data, usingType);
        var builtPlan = planDirector.BuildPlan();
        
        await using(var context = new FitEntitiesContext())
        {
            User? user = await context.Users.FirstOrDefaultAsync(c => c.Id == userId);
            
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
    
    public async Task<List<PlanComment>?> GetPlanComments(Guid planId, Guid userId)
    {
        await using (var context = new FitEntitiesContext())
        {
            var fitPlans = await context.Plans
                .Include(x=>x.Comments)
                .FirstOrDefaultAsync(e => e.Id == planId);
            
            if(fitPlans.UserId != userId && !fitPlans.isPublic) return null;
            
            return fitPlans.Comments.ToList();
        }
    }
    
    public async Task<PlanRating?> GetPlanRating(Guid planId)
    {
        await using (var context = new FitEntitiesContext())
        {
            var fitPlan = await context.Plans
                .Include(c => c.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == planId);
            
            if(fitPlan == null) return null;
            if(!fitPlan.isPublic) return null;
            
            return new PlanRating(
                fitPlan.Comments.Average(c=>c.Rating), 
                fitPlan.Comments.Count
                );
        }
    }
}