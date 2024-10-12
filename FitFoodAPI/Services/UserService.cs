using System.Net;
using FitFoodAPI.Database;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitFoodAPI.Services;

public class UserService()
{

    public async Task<HttpStatusCode> CreateUser(User user)
    {
        user.Plans = new List<FitPlan>();
        user.Datas = new List<FitData>();
        await using (var context = new FitEntitiesContext())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return HttpStatusCode.Created;
        }
    }
    public async Task<List<User>> GetAll()
    {
        await using (var context = new FitEntitiesContext())
        {
            return await context.Users
                .AsNoTracking()
                .Include(x => x.Plans)
                .Include(x => x.Datas)
                .ToListAsync();
        }
    }

    public async Task<User?> GetById(Guid id)
    {
        await using (var context = new FitEntitiesContext())
        {
            return await context.Users.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
    
    public async Task<HttpStatusCode> AddData(Guid userId, FitData data)
    {
        await using(var context = new FitEntitiesContext())
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Id == userId);
            
            if (user == null) return HttpStatusCode.NotFound;
            
            user.Datas.Add(data);
            context.Users.Update(user);
            
            await context.SaveChangesAsync();
        }
        return HttpStatusCode.OK;
    }
}