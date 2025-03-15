using Microsoft.EntityFrameworkCore;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services;

public sealed class DatabaseManager : DbContext
{
    public DbSet<Order> Orders { get; set; }
    
    public DatabaseManager()
    {
        Database.EnsureCreated();
    }
    
    public int AddOrder(Order order)
    {
        Order entity = Orders.Add(order).Entity;
        SaveChanges();
        return entity.Id;
    }
    
    public Order? GetOrderById(int orderId)
    {
        return Orders.FirstOrDefault(o => o.Id == orderId);
    }
    
    public bool UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null) return false;

        order.Status = newStatus;
        SaveChanges();
        return true;
    }
    
    public List<Order> GetOrders() => Orders.ToList();

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
            entity.Property(e => e.ProductName).IsRequired();
            entity.Property(e => e.ShippingAddress).IsRequired();
        });
    }
}