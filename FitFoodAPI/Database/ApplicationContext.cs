using System.Data.Entity;
using FitFoodAPI.Models;
using OpenFoodFacts4Net.Json.Data;

namespace FitFoodAPI.Database;

public class ApplicationContext : DbContext
{
    public DbSet<FitData> FitDatas { get; set; }
    public DbSet<ProductData> Products { get; set; }

    public ApplicationContext() : base("DefaultConnection")
    {
    }
}