using OrderProcessingApp.Dialogs;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models.Enums;
using OrderProcessingApp.Services.PageHandlers;

namespace OrderProcessingApp.Services;

public class DialogHandler
{
    private IPageHandler _currentPageHandler;
    private Dictionary<Page, Func<IPageHandler>> _pageHandlers;
    
    public DialogHandler()
    {
        _pageHandlers = new Dictionary<Page, Func<IPageHandler>>
        {
            {Page.MainMenu, () => new MainMenuHandler(ChangePage)},
            {Page.CreateOrder, () => new CreateOrderHandler(ChangePage)},
            {Page.SendToWarehouse, () => new SendToWarehouseHandler(ChangePage)},
            {Page.OrdersList, () => new OrdersListHandler(ChangePage)},
        };
    }

    public void Start()
    {
        WelcomeUser();
        UserInputLoop();
    }
    
    private void ChangePage(Page page)
    {
        if (_currentPageHandler != null)
            Console.Clear();
        
        if (!_pageHandlers.TryGetValue(page, out var newPageHandlerInitializer))
            throw new ArgumentException($"Page handler for page {page.ToString()} not found");

        IPageHandler newPageHandler = newPageHandlerInitializer();
        newPageHandler.DisplayOptions();
        _currentPageHandler = newPageHandler;
    }
    
    private void WelcomeUser()
    {
        Console.WriteLine(SharedDialogs.Welcome + "\n" + SharedDialogs.Info);
        ChangePage(Page.MainMenu);
    }

    private void UserInputLoop()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? string.Empty;
            
            if (input.ToUpper() == "EXIT")
                Program.Close();

            if (input.ToUpper() == "BACK")
            {
                ChangePage(Page.MainMenu);
                continue;
            } 
            
            _currentPageHandler.HandleUserInput(input);
        }
    }
}