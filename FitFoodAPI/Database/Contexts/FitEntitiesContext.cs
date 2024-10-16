using FitFoodAPI.Database.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using FitFoodAPI.Models;

namespace FitFoodAPI.Database.Contexts;

public class FitEntitiesContext : DbContext
{
    public DbSet<FitData> Datas { get; set; }
    public DbSet<FitPlan> Plans { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlanComment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=admin;Database=fitfood_db");
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