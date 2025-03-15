using OrderProcessingApp.Dialogs;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services.PageHandlers;

public class SendToShippingHandler: IPageHandler
{ 
    private readonly List<Action<string>> _dialogSteps;
    private readonly Action<Page> _changePageFunction;

    public SendToShippingHandler(Action<Page> changePageFunction)
    {
        _changePageFunction = changePageFunction;
        _dialogSteps = 
        [
            SendOrderToShipping
        ];
    }

    private void SendOrderToShipping(string userInput)
    {
        if (!int.TryParse(userInput, out int orderId))
        {
            Console.WriteLine(SharedDialogs.NoOrdersWithProvidedId);
            return;
        }
        
        Order? order = Program.DatabaseManager.GetOrderById(orderId);
        if (order == null)
        {
            Console.WriteLine(SharedDialogs.NoOrdersWithProvidedId);
            return;
        }
        
        if (order.Status != OrderStatus.InWarehouse)
        {
            Console.WriteLine(SendToShippingDialog.OrderCantBeProcessed);
            return;
        }
        
        Program.DatabaseManager.UpdateOrderStatus(orderId, OrderStatus.InShipping);
        Console.WriteLine(SendToShippingDialog.OrderSentToShipping);
    }

    public void DisplayOptions() => Console.Write(SendToShippingDialog.InitialMessage);

    public void HandleUserInput(string userInput)
    {
        if (_dialogSteps.Any())
        {
            _dialogSteps[0](userInput);
            _dialogSteps.RemoveAt(0);
            Console.Write(SharedDialogs.AnotherOrderOrReturn);
            return;
        }
        
        switch (userInput.ToLower())
        {
            case "y":
                _changePageFunction(Page.SendToShipping);
                return;
            case "n":
                _changePageFunction(Page.MainMenu);
                return;
            default:
                Console.Write(SharedDialogs.InvalidOption);
                break;
        }
    }
}