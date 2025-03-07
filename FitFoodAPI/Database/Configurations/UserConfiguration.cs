﻿using FitFoodAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitFoodAPI.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.Datas)
            .WithOne(x => x.User);
        builder.HasMany(x => x.Plans)
            .WithOne(x => x.User);
        builder.HasMany(x => x.FeedActs)
            .WithOne(x => x.User);
        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(e => e.Email).IsUnique();
    }
}