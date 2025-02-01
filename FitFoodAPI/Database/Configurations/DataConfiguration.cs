using FitFoodAPI.Models;
using FitFoodAPI.Models.Fit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class DataConfiguration : IEntityTypeConfiguration<FitData>
{
    public void Configure(EntityTypeBuilder<FitData> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.User)
            .WithMany(x => x.Datas)
            .HasForeignKey(x => x.UserId);

    }
}