using OrderProcessingApp.Dialogs;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services.PageHandlers;

public class MainMenuHandler(Action<Page> changePageFunction) : IPageHandler
{
    public void DisplayOptions() => Console.Write(MainMenuDialog.Options);
    public void HandleUserInput(string userInput)
    {
        if (!int.TryParse(userInput, out int choice))
        {
            Console.Write(SharedDialogs.InvalidOption);
            return;
        }

        switch (choice)
        {
            case 1:
                changePageFunction(Page.CreateOrder);
                break;
            case 2:
                changePageFunction(Page.SendToWarehouse);
                break;
            case 3:
                changePageFunction(Page.SendToShipping);
                break;
            case 4:
                changePageFunction(Page.OrdersList);
                break;
            case 5:
                Program.Close();
                break;
            default:
                Console.Write(SharedDialogs.InvalidOption);
                break;
        }
    }
}