using FitFoodAPI.Database.Contexts;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace FitFoodAPI.Services;

public class CommentService
{
    public async Task<int> AddComment(PlanComment comment)
    {
        await using(var context = new FitEntitiesContext())
        {
            var plan = await context.Plans.FirstOrDefaultAsync(c => c.Id == comment.PlanId);
            
            if (plan == null) return 0;
            
            plan.Comments.Add(comment);
            context.Plans.Update(plan);
            
            await context.SaveChangesAsync();
        }
        return 1;
    }

    public async Task<int> DeleteComment(Guid id)
    {
        await using(var context = new FitEntitiesContext())
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            
            if (comment == null) return 0;
            
            comment.IsDeleted = true;
            context.Comments.Update(comment);
            
            await context.SaveChangesAsync();
        }
        return 1;
    }
    public async Task<int> EditComment(Guid id, EditCommentRequest newData)
    {
        await using(var context = new FitEntitiesContext())
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            
            if (comment == null) return 0;
            
            comment.Text = newData.Text;
            comment.Rating = newData.Rating;
            comment.EditedAt = DateTime.UtcNow.ToString("dd.MM.yyyy");
            context.Comments.Update(comment);
            
            await context.SaveChangesAsync();
        }
        return 1;
    }
}