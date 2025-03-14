namespace OrderProcessingApp.Consts;

public static class DialogOptions
{
    public const string Welcome = "Welcome to the Order Processing App!";
    public const string InvalidOption = "Invalid option";
    public const string ChooseOption = "Choose an option: ";

    private const string CreateOrder = "1. Create sample order";
    private const string SendToWarehouse = "2. Send order to warehouse";
    private const string SendToShipping = "3. Send order to shipping";
    private const string ViewOrders = "4. View orders";
    private const string Exit = "5. Exit";

    public static string GetMainMenuOptions() =>
        "Options (1-5):\n" +
        CreateOrder + "\n" +
        SendToWarehouse + "\n" +
        SendToShipping + "\n" +
        ViewOrders + "\n" +
        Exit + "\n" +
        ChooseOption;

}