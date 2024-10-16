using FitFoodAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<PlanComment>
{
    public void Configure(EntityTypeBuilder<PlanComment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.FitPlan)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PlanId);
    }
}