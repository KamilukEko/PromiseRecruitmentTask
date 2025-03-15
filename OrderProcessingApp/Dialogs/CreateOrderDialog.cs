namespace OrderProcessingApp.Dialogs;

public class CreateOrderDialog
{
    public const string InitialMessage = "Please enter the following information to create an order";
    public const string OrderValue = "1. Order amount: ";
    public const string ProductName = "2. Product name: ";
    public const string CustomerType = "3. Customer type (Company, Person): ";
    public const string DeliveryAddress = "4. Delivery address: ";
    public const string PaymentMethod = "5. Payment method (CreditCard, Cash): ";
    
    public const string InvalidOrderAmount = "Invalid order amount";
    public const string InvalidClientType = "Invalid customer type";
    public const string InvalidPaymentMethod = "Invalid payment method";
    public const string InvalidProductName = "Invalid product name";
    public const string InvalidShippingAddress = "Invalid shipping address";
    
    public static string OrderCreated(int orderId) => $"Order with id - {orderId} was created successfully!";
    public static string OrderCreationFailed(int orderId) => $"Creation of order with id - {orderId} failed!";
    
    public const string AnotherOrderOrReturn = "Do you want to create another order? (Y/N): ";
}