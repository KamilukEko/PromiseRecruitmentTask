using OrderProcessingApp.Consts;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services.PageHandlers;

public class MainMenuHandler(Action<Page> changePageFunction) : IPageHandler
{
    public void DisplayOptions() => Console.Write(DialogOptions.GetMainMenuOptions());
    public void HandleUserInput(string userInput)
    {
        if (!int.TryParse(userInput, out int choice))
        {
            Console.Write(DialogOptions.InvalidOption);
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
                Console.Write(DialogOptions.InvalidOption);
                break;
        }
    }
}