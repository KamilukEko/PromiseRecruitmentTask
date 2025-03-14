using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Models;

public class Order
{
    public decimal TotalAmount { get; }
    public string ProductName { get; }
    public ClientType ClientType { get; }
    public string ShippingAddress { get; }
    public PaymentMethod PaymentMethod { get; }
}