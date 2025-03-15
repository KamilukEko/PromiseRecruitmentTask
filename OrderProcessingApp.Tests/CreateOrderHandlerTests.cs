using System.Globalization;
using NUnit.Framework;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;
using System.Reflection;
using OrderProcessingApp.Services.PageHandlers;

namespace OrderProcessingApp.Tests
{
    [TestFixture]
    public class CreateOrderHandlerTests
    {
        private CreateOrderHandler _handler;
        private MethodInfo _validateOrderMethod;
        private MethodInfo _createOrderMethod;
        private FieldInfo _orderDetailsValuesField;

        [SetUp]
        public void Setup()
        {
            _handler = new CreateOrderHandler(_ => { });
            _validateOrderMethod = typeof(CreateOrderHandler).GetMethod("ValidateOrder",
                BindingFlags.NonPublic | BindingFlags.Instance)!;
            _createOrderMethod = typeof(CreateOrderHandler).GetMethod("CreateOrder",
                BindingFlags.NonPublic | BindingFlags.Instance)!;
            _orderDetailsValuesField = typeof(CreateOrderHandler).GetField("_orderDetailsValues",
                BindingFlags.NonPublic | BindingFlags.Instance)!;
        }

        [TestCase("2137", "Product", "Person", "Mazowiecka 1", "Cash", true)]
        [TestCase("21.37", "Product", "Person", "Smolna 1", "Cash", true)]
        [TestCase("21,37", "Product", "Company", "Mokotowska 3", "Cash", true)]
        [TestCase("2137", "Product", "Person", "", "CreditCard", false)]
        [TestCase("21.37", "", "Person", "Łucka 2", "CreditCard", false)]
        [TestCase("21,37", "Product", "Company", "Jasna 2", "Crypto", false)]
        [TestCase("one", "Product", "Company", "Mordor 7", "CreditCard", false)]
        [TestCase("2137", "Product", "Family", "Nowoursynowska 5", "CreditCard", false)]
        [TestCase("-1", "Product", "Company", "Marszałkowska 9", "CreditCard", false)]
        public void ValidateOrder_ValidatesInput_Correctly(string amount, string product, string clientType,
            string address, string paymentType, bool expectedResult)
        {
            _orderDetailsValuesField.SetValue(_handler, new List<string> { amount, product, clientType, address, paymentType });

            var parameters = new object?[] { null };
            bool result = (bool)_validateOrderMethod.Invoke(_handler, parameters)!;
            var order = (Order)parameters[0]!;
            
            Assert.That(result, Is.EqualTo(expectedResult));

            if (expectedResult)
            {
                Assert.That(order.Status == OrderStatus.New);
            }
            else
            {
                Assert.That(order.Status == OrderStatus.Error);
            }
        }
        
        [Test]
        public void CreateOrder_AddsNewOrderToDatabase_Successfully()
        {
            List<string> orderDetails = [
                Utils.GenerateRandomDecimal().ToString(CultureInfo.InvariantCulture), 
                Utils.GenerateRandomString(), 
                Utils.GenerateRandomEnum<ClientType>().ToString(), 
                Utils.GenerateRandomString(),
                Utils.GenerateRandomEnum<PaymentMethod>().ToString()
            ];
            _orderDetailsValuesField.SetValue(_handler, orderDetails);
            
            var parameters = new object?[] { };
            _createOrderMethod.Invoke(_handler, parameters);

            var savedOrder = Program.DatabaseManager.GetOrders().Last();
            Assert.Multiple(() =>
            {
                Assert.That(savedOrder.TotalAmount, Is.EqualTo(decimal.Parse(orderDetails[0], CultureInfo.InvariantCulture)));
                Assert.That(savedOrder.ProductName, Is.EqualTo(orderDetails[1]));
                Assert.That(savedOrder.ClientType, Is.EqualTo(Enum.Parse<ClientType>(orderDetails[2])));
                Assert.That(savedOrder.ShippingAddress, Is.EqualTo(orderDetails[3]));
                Assert.That(savedOrder.PaymentMethod, Is.EqualTo(Enum.Parse<PaymentMethod>(orderDetails[4])));
                Assert.That(savedOrder.Status, Is.EqualTo(OrderStatus.New));
            });
        }
        
        [Test]
        public void CreateOrder_AddsErrorOrderToDatabase_Successfully()
        {
            List<string> orderDetails = [
                Utils.GenerateRandomDecimal().ToString(CultureInfo.InvariantCulture), 
                Utils.GenerateRandomString(), 
                Utils.GenerateRandomString(), 
                Utils.GenerateRandomString(),
                Utils.GenerateRandomEnum<PaymentMethod>().ToString()
            ];
            _orderDetailsValuesField.SetValue(_handler, orderDetails);
            
            var parameters = new object?[] { };
            _createOrderMethod.Invoke(_handler, parameters);

            var savedOrder = Program.DatabaseManager.GetOrders().Last();
            Assert.Multiple(() =>
            {
                Assert.That(savedOrder.TotalAmount, Is.EqualTo(decimal.Parse(orderDetails[0], CultureInfo.InvariantCulture)));
                Assert.That(savedOrder.ProductName, Is.EqualTo(orderDetails[1]));
                Assert.That(savedOrder.ClientType, Is.EqualTo(ClientType.Unknown));
                Assert.That(savedOrder.ShippingAddress, Is.EqualTo(orderDetails[3]));
                Assert.That(savedOrder.PaymentMethod, Is.EqualTo(Enum.Parse<PaymentMethod>(orderDetails[4])));
                Assert.That(savedOrder.Status, Is.EqualTo(OrderStatus.Error));
            });
        }
    }
}