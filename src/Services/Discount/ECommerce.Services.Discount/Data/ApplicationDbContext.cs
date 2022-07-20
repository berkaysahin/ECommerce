using Microsoft.EntityFrameworkCore;

namespace ECommerce.Services.Discount.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Models.Discount>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }

    public DbSet<Models.Discount> Discounts { get; set; }
}
