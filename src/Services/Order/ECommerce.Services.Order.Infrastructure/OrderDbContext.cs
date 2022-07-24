using Microsoft.EntityFrameworkCore;

namespace ECommerce.Services.Order.Infrastructure;

public class OrderDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "ordering";

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
    public DbSet<Domain.OrderAggregate.OrderItem> OrderItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Order", DEFAULT_SCHEMA);
        modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItem", DEFAULT_SCHEMA);

        modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(item => item.Price).HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(item => item.Address).WithOwner();
        
        base.OnModelCreating(modelBuilder);
    }
}
