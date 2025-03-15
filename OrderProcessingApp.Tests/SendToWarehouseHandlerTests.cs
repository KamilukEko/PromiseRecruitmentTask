using System.Reflection;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;
using OrderProcessingApp.Services.PageHandlers;

namespace OrderProcessingApp.Tests;

[TestFixture]
public class SendToWarehouseHandlerTests
{
    private SendToWarehouseHandler _handler;
    private MethodInfo _sendOrderToWarehouse;
    
    [SetUp]
    public void Setup()
    {
        _handler = new SendToWarehouseHandler(_ => { });
        _sendOrderToWarehouse = typeof(SendToWarehouseHandler).GetMethod("SendOrderToWarehouse",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
    }
    
    [TestCase(PaymentMethod.CreditCard, -1, OrderStatus.Error, OrderStatus.Error)]
    [TestCase(PaymentMethod.CreditCard, 2500, OrderStatus.New, OrderStatus.InWarehouse)]
    [TestCase(PaymentMethod.CreditCard, 2501, OrderStatus.InWarehouse, OrderStatus.InWarehouse)]
    [TestCase(PaymentMethod.CreditCard, 2501, OrderStatus.New, OrderStatus.InWarehouse)]
    [TestCase(PaymentMethod.Cash, -1, OrderStatus.Error, OrderStatus.Error)]
    [TestCase(PaymentMethod.Cash, 2500, OrderStatus.New, OrderStatus.InWarehouse)]
    [TestCase(PaymentMethod.Cash, 2501, OrderStatus.InWarehouse, OrderStatus.InWarehouse)]
    [TestCase(PaymentMethod.Cash, 2501, OrderStatus.New, OrderStatus.Returned)]
    public void SendOrderToWarehouse_SetsStatus_Correctly(PaymentMethod paymentMethod, decimal amount, OrderStatus currentStatus, OrderStatus expectedStatus)
    {
        var order = new Order
        {
            ProductName = Utils.GenerateRandomString(),
            ShippingAddress = Utils.GenerateRandomString(),
            ClientType = Utils.GenerateRandomEnum<ClientType>(),
            Status = currentStatus,
            PaymentMethod = paymentMethod,
            TotalAmount = amount
        };
        int orderId = Program.DatabaseManager.AddOrder(order);
        
        _sendOrderToWarehouse.Invoke(_handler, [orderId.ToString()]);
        
        var updatedOrder = Program.DatabaseManager.GetOrderById(orderId);
        Assert.That(updatedOrder?.Status, Is.EqualTo(expectedStatus));
    }
}