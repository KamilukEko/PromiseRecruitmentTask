using System.Text;
using OrderProcessingApp.Dialogs;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services.PageHandlers;

public class OrdersListHandler(Action<Page> changePageFunction): IPageHandler
{
    private string GenerateOrdersString(List<Order> orders)
    {
        var ordersListString = new StringBuilder();
        foreach (var order in orders)
        {
            ordersListString.AppendLine(
                $"{order.Id} | {order.TotalAmount:C} | {order.ProductName} | {order.ClientType} | {order.ShippingAddress} | {order.PaymentMethod} | {order.Status}");
        }

        return ordersListString.ToString();
    }
    
    public void DisplayOptions()
    {
        List<Order> orders = Program.DatabaseManager.GetOrders();

        if (!orders.Any())
        {
            Console.WriteLine(OrdersListDialog.NoOrders);
        }
        else
        {
            Console.WriteLine(OrdersListDialog.InitialMessage);
            Console.WriteLine(OrdersListDialog.Header);
            Console.Write(GenerateOrdersString(orders));
        }
        
        Console.Write(SharedDialogs.GoBackMessage);
    }

    public void HandleUserInput(string userInput) => changePageFunction(Page.MainMenu);
}