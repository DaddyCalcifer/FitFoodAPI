using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using FitFoodAPI.Database;
using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Enums;
using FitFoodAPI.Services.Builders;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using FitFoodAPI.Models.Requests;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<string?> Authorize(AuthRequest request)
    {
        await using (var context = new FitEntitiesContext())
        {
            var _user = await context.Users
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
                if (!BCrypt.Net.BCrypt.Verify(request.Password, _user.Password)) return null;
                var claims = new List<Claim> { new Claim(ClaimTypes.Sid, _user.Id.ToString()) };
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
                return new JwtSecurityTokenHandler().WriteToken(jwt);
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
                .Include(u => u.Datas)
                .Include(u => u.Plans)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
    public async Task<User?> GetById_lite(Guid id)
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