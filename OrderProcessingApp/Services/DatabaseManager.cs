using Microsoft.EntityFrameworkCore;
using OrderProcessingApp.Models;

namespace OrderProcessingApp.Services;

public sealed class DatabaseManager : DbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public DatabaseManager()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=orderprocessing.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).IsRequired(); ;
            entity.Property(e => e.ClientType).IsRequired();
            entity.Property(e => e.PaymentMethod).IsRequired();
            entity.Property(e => e.Status).IsRequired();
        });
    }
}