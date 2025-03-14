using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Models;

public class Order
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string ProductName { get; set; }
    public string ShippingAddress { get; set; }
    public ClientType ClientType { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderStatus Status { get; set; }
}