﻿using FitFoodAPI.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using FitFoodAPI.Models;
using FitFoodAPI.Models.Fit;
using FitFoodAPI.Models.Nutrition;
using FitFoodAPI.Models.Sport;

namespace FitFoodAPI.Database.Contexts;

public class FitEntitiesContext : DbContext
{
    public DbSet<FitData> Datas { get; set; }
    public DbSet<FitPlan> Plans { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlanComment> Comments { get; set; }
    public DbSet<FeedAct> FeedActs { get; set; }
    public DbSet<ProductData> Products { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<TrainingPlan> TrainingPlans { get; set; }
    public DbSet<Set> Sets { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseProgress> ExerciseProgress { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=admin;Database=fitfood_db");
            optionsBuilder.UseNpgsql("Host=dpg-d02m5ijuibrs73b1hd10-a.frankfurt-postgres.render.com;Port=5432;Username=admin;Password=F7pIV7sBgzs8NzzXAqsoQa4fPE0V7vYw;Database=fitfood_db_ymbb");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DataConfiguration());
        modelBuilder.ApplyConfiguration(new PlanConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}