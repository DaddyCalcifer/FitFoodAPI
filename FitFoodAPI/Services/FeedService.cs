using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Models.Nutrition;
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
    public async Task<List<FeedAct>> GetAllByDateAndType(Guid userId, string date, FeedType type)
    {
        await using(var context = new FitEntitiesContext())
        {
            return await context.FeedActs
                .Where(c => c.UserId == userId 
                            && c.Date.Trim() == date.Trim()
                            && c.FeedType == type)
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
                AteKcal = kcal,
                AteDinner = getEatingsKcal(userId, date, FeedType.Dinner),
                AteLunch = getEatingsKcal(userId, date, FeedType.Lunch),
                AteBreakfast = getEatingsKcal(userId, date, FeedType.Breakfast),
                AteOther = getEatingsKcal(userId, date, FeedType.Other),
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

    double getEatingsKcal(Guid userId, string date, FeedType feedType)
    {
        using (var context = new FitEntitiesContext())
        {
            var query = context.FeedActs
                .Where(c => c.UserId == userId && c.Date == date.Trim() && c.FeedType == feedType);

            double sum = 0;

            if (query.Any())
            {
                var feeds =  query.ToList();
                foreach (var feed in feeds)
                {
                    sum += feed.Kcal;
                }
            }
            return sum;
        }
    }
    public async Task<List<ProductData>?> SearchProducts(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        await using (var context = new FitEntitiesContext())
        {
            var products = await context.Products.ToListAsync();

            // Разбиваем запрос на отдельные слова
            var queryWords = name.ToLower().Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var filteredProducts = products
                .Select(p => new
                {
                    Product = p,
                    SimilarityScore = CalculateProductSimilarity(p.Name, queryWords) // Считаем рейтинг похожести
                })
                .OrderByDescending(p => p.SimilarityScore) // Сортируем по рейтингу схожести
                .Take(10) // Оставляем 10 лучших вариантов
                .Select(p => p.Product)
                .ToList();

            return filteredProducts.Count == 0 ? null : filteredProducts;
        }
    }
    private int LevenshteinDistance(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1)) return s2.Length;
        if (string.IsNullOrEmpty(s2)) return s1.Length;

        var d = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= s2.Length; j++) d[0, j] = j;

        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[s1.Length, s2.Length];
    }
    private double CalculateProductSimilarity(string productName, string[] queryWords)
    {
        var productWords = productName.ToLower().Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

        double totalScore = 0;
        int comparisons = 0;

        foreach (var queryWord in queryWords)
        {
            double bestMatch = productWords
                .Select(productWord => 1.0 - (double)LevenshteinDistance(queryWord, productWord) / Math.Max(queryWord.Length, productWord.Length))
                .Max();

            totalScore += bestMatch;
            comparisons++;
        }

        return comparisons > 0 ? totalScore / comparisons : 0;
    }
}