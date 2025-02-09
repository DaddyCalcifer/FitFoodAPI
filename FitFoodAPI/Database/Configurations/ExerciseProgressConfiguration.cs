using FitFoodAPI.Models.Sport;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class ExerciseProgressConfiguration : IEntityTypeConfiguration<ExerciseProgress>
{
    public void Configure(EntityTypeBuilder<ExerciseProgress> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasMany(e => e.Sets)
            .WithOne()
            .HasForeignKey(e => e.ExerciseProgressId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}