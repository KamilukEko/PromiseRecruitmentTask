namespace OrderProcessingApp.Dialogs;

public class SendToShippingDialog
{
    public const string InitialMessage = "Enter order ID to send it to shipment: ";
    public const string OrderCantBeProcessed = "Order with this ID is not in a warehouse and can't be shipped";
    public const string OrderSentToShipping= "Order sent to shipping successfully!";
}