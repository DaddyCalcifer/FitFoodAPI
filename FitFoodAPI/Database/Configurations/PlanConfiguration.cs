using FitFoodAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<FitPlan>
{
    public void Configure(EntityTypeBuilder<FitPlan> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.User)
            .WithMany(x => x.Plans)
            .HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.Comments)
            .WithOne(x => x.FitPlan);
    }
}