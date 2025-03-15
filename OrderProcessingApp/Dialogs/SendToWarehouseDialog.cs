namespace OrderProcessingApp.Dialogs;

public class SendToWarehouseDialog
{
    public const string InitialMessage = "Enter order ID to send it to warehouse: ";
    public const string OrderCantBeProcessed = "This order can't be sent to warehouse. It's already processed or has an error";
    public const string NoOrdersWithProvidedId = "There is no order with this ID";
    public const string AnotherOrderOrReturn = "Do you want to send another order? (Y/N): ";
    public const string OrderReturnToClient = "Value of this order exceeds 2500 and payment method is cash. This order needs to be returned to client";
    public const string OrderSentToWarehouse = "Order sent to warehouse successfully!";
}