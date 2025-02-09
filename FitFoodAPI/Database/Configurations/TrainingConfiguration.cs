using FitFoodAPI.Models;
using FitFoodAPI.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class TrainingConfiguration : IEntityTypeConfiguration<Training>
{
    public void Configure(EntityTypeBuilder<Training> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Exercises)
            .WithOne(x => x.Training)
            .OnDelete(DeleteBehavior.Cascade);;
    }
}