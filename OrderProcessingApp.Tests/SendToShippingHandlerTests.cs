using System.Reflection;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;
using OrderProcessingApp.Services.PageHandlers;

namespace OrderProcessingApp.Tests;

[TestFixture]
public class SendToShippingHandlerTests
{
    private SendToShippingHandler _handler;
    private MethodInfo _sendOrderToWarehouse;
    
    [SetUp]
    public void Setup()
    {
        _handler = new SendToShippingHandler(_ => { });
        _sendOrderToWarehouse = typeof(SendToShippingHandler).GetMethod("SendOrderToShipping",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
    }
    
    [TestCase(OrderStatus.Error, OrderStatus.Error)]
    [TestCase(OrderStatus.New, OrderStatus.New)]
    [TestCase(OrderStatus.Closed, OrderStatus.Closed)]
    [TestCase(OrderStatus.Returned, OrderStatus.Returned)]
    [TestCase(OrderStatus.InShipping, OrderStatus.InShipping)]
    [TestCase(OrderStatus.InWarehouse, OrderStatus.InShipping)]
    public void SendOrderToWarehouse_SetsStatus_Correctly(OrderStatus currentStatus, OrderStatus expectedStatus)
    {
        var order = new Order
        {
            ProductName = Utils.GenerateRandomString(),
            ShippingAddress = Utils.GenerateRandomString(),
            ClientType = Utils.GenerateRandomEnum<ClientType>(),
            Status = currentStatus,
            PaymentMethod = Utils.GenerateRandomEnum<PaymentMethod>(),
            TotalAmount = Utils.GenerateRandomDecimal()
        };
        int orderId = Program.DatabaseManager.AddOrder(order);
        
        _sendOrderToWarehouse.Invoke(_handler, [orderId.ToString()]);
        
        var updatedOrder = Program.DatabaseManager.GetOrderById(orderId);
        Assert.That(updatedOrder?.Status, Is.EqualTo(expectedStatus));
    }
}