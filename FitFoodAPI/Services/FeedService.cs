using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace FitFoodAPI.Services;

public class FeedService
{
    public async Task<int> AddFeed(FeedAct act)
    {
        await using(var context = new FitEntitiesContext())
        { 
            act.Date = DateTime.UtcNow.ToString("dd.MM.yyyy");
            context.FeedActs.Add(act);
            
            await context.SaveChangesAsync();
        }
        return 1;
    }

    public async Task<bool> DeleteFeed(Guid feedId, Guid userId)
    {
        await using(var context = new FitEntitiesContext())
        {
            var feed = await context.FeedActs
                .FirstOrDefaultAsync(c => c.Id == feedId);
            
            if (feed == null) return false;
            if(feed.UserId != userId)
                    return false;
            
            context.FeedActs.Remove(feed);
            
            await context.SaveChangesAsync();
        }
        return true;
    }
    public async Task<FeedAct?> GetById(Guid feedId, Guid userId)
    {
        await using(var context = new FitEntitiesContext())
        {
            var feed = await context.FeedActs
                .FirstOrDefaultAsync(c => c.Id == feedId);
            
            if (feed == null) return null;
            return feed.UserId != userId ? null : feed;
        }
    }
    public async Task<List<FeedAct>> GetAll(Guid userId)
    {
        await using(var context = new FitEntitiesContext())
        {
            return await context.FeedActs.Where(c => c.UserId == userId).ToListAsync();
        }
    }
    public async Task<List<FeedAct>> GetAllByDate(Guid userId, string date)
    {
        await using(var context = new FitEntitiesContext())
        {
            return await context.FeedActs
                .Where(c => c.UserId == userId 
                            && c.Date.Trim() == date.Trim())
                .ToListAsync();
        }
    }
    public async Task<FeedStats> GetFeedStatsByType(Guid userId, string date, FeedType feedType)
    {
        await using (var context = new FitEntitiesContext())
        {
            var query = context.FeedActs
                .Where(c => c.UserId == userId && c.Date == date.Trim() && c.FeedType == feedType);

            double protein = 0.0, carb = 0.0, fat = 0.0, kcal = 0.0;

            if (query.Any())
            {
                var feeds = await query.ToListAsync();
                foreach (var feed in feeds)
                {
                    protein += feed.Protein;
                    carb += feed.Carb;
                    fat += feed.Fat;
                    kcal += feed.Kcal;
                }
            }

            return new FeedStats
            {
                AteProtein = protein,
                AteCarb = carb,
                AteFat = fat,
                AteKcal = kcal
            };
        }
    }
    public async Task<FeedStats> GetAllFeedStats(Guid userId, string date)
    {
        await using (var context = new FitEntitiesContext())
        {
            var query = context.FeedActs
                .Where(c => c.UserId == userId && c.Date == date.Trim());

            double protein = 0.0, carb = 0.0, fat = 0.0, kcal = 0.0;

            if (query.Any())
            {
                var feeds = await query.ToListAsync();
                foreach (var feed in feeds)
                {
                    protein += feed.Protein;
                    carb += feed.Carb;
                    fat += feed.Fat;
                    kcal += feed.Kcal;
                }
            }

            return new FeedStats
            {
                AteProtein = protein,
                AteCarb = carb,
                AteFat = fat,
                AteKcal = kcal
            };
        }
    }
    
    public async Task<bool> EditFeed(Guid id, Guid userId, FeedAct newData)
    {
        await using(var context = new FitEntitiesContext())
        {
            var feed = await context.FeedActs.FirstOrDefaultAsync(c => c.Id == id);
            
            if (feed == null) return false;
            if (feed.UserId != userId) return false;
            
            feed.Name = newData.Name;
            feed.Fat100 = newData.Fat100;
            feed.Protein100 = newData.Protein100;
            feed.Carb100 = newData.Carb100;
            feed.Kcal100 = newData.Kcal100;
            feed.FeedType = newData.FeedType;
            
            context.FeedActs.Update(feed);
            
            await context.SaveChangesAsync();
        }
        return true;
    }
}