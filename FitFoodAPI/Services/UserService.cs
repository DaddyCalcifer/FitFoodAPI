using System.Net;
using FitFoodAPI.Database;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services.Builders;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace FitFoodAPI.Services;

public class UserService()
{

    public async Task<User> CreateUser(User user)
    {
        user.Plans = new List<FitPlan>();
        user.Datas = new List<FitData>();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await using (var context = new FitEntitiesContext())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }
    }

    public async Task<Guid?> Authorize(AuthRequest request)
    {
        await using (var context = new FitEntitiesContext())
        {
            User? _user = await context.Users
                .AsNoTracking()
                .Where(x => 
                    x.Username == request.Login || x.Email == request.Login)
                .FirstOrDefaultAsync();
            if (_user == null)
            {
                return null;
            }
            else
            {
                if (BCrypt.Net.BCrypt.Verify(request.Password, _user.Password))
                    return _user.Id;
            }
        }

        return null;
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
            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
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