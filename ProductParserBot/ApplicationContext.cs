using Microsoft.EntityFrameworkCore;

namespace ProductParserBot;

public class ApplicationDbContext : DbContext
{
    public DbSet<ProductData> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=admin;Password=admin;Database=fitfood_db");
    }
}
