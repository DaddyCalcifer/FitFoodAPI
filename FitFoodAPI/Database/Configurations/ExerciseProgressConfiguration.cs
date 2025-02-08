using FitFoodAPI.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class ExerciseProgressConfiguration : IEntityTypeConfiguration<ExerciseProgress>
{
    public void Configure(EntityTypeBuilder<ExerciseProgress> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Sets)
            .WithOne(x => x.ExerciseProgress);
    }
}