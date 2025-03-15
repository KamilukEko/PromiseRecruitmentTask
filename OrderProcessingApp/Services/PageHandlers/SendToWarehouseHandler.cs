using System.Text;
using OrderProcessingApp.Dialogs;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services.PageHandlers;

public class SendToWarehouseHandler: IPageHandler
{ 
    private readonly List<Action<string>> _dialogSteps;
    private readonly Action<Page> _changePageFunction;

    public SendToWarehouseHandler(Action<Page> changePageFunction)
    {
        _changePageFunction = changePageFunction;
        _dialogSteps = 
        [
            SendOrderToWarehouse
        ];
    }

    private void SendOrderToWarehouse(string userInput)
    {
        if (!int.TryParse(userInput, out int orderId))
        {
            Console.WriteLine(SendToWarehouseDialog.NoOrdersWithProvidedId);
            return;
        }
        
        Order? order = Program.DatabaseManager.GetOrderById(orderId);
        if (order == null)
        {
            Console.WriteLine(SendToWarehouseDialog.NoOrdersWithProvidedId);
            return;
        }
        
        if (order.Status != OrderStatus.New)
        {
            Console.WriteLine(SendToWarehouseDialog.OrderCantBeProcessed);
            return;
        }
        
        if (order is { PaymentMethod: PaymentMethod.Cash, TotalAmount: > 2500 })
        {
            Console.WriteLine(SendToWarehouseDialog.OrderReturnToClient);
            Program.DatabaseManager.UpdateOrderStatus(orderId, OrderStatus.Returned);
            return;
        }
        
        Program.DatabaseManager.UpdateOrderStatus(orderId, OrderStatus.InWarehouse);
        Console.WriteLine(SendToWarehouseDialog.OrderSentToWarehouse);
    }

    public void DisplayOptions() => Console.Write(SendToWarehouseDialog.InitialMessage);

    public void HandleUserInput(string userInput)
    {
        if (_dialogSteps.Any())
        {
            _dialogSteps[0](userInput);
            _dialogSteps.RemoveAt(0);
            Console.Write(SendToWarehouseDialog.AnotherOrderOrReturn);
            return;
        }
        
        switch (userInput.ToLower())
        {
            case "y":
                _changePageFunction(Page.SendToWarehouse);
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