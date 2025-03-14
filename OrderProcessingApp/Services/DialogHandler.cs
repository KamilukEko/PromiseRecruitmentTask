using OrderProcessingApp.Consts;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models.Enums;
using OrderProcessingApp.Services.PageHandlers;

namespace OrderProcessingApp.Services;

public class DialogHandler
{
    private IPageHandler _currentPageHandler;
    private Dictionary<Page, IPageHandler> _pageHandlers;
    
    public DialogHandler()
    {
        _pageHandlers = new Dictionary<Page, IPageHandler>
        {
            {Page.MainMenu, new MainMenuHandler(ChangePage)}
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
        
        _currentPageHandler = _pageHandlers[page];
        _currentPageHandler.DisplayOptions();
    }
    
    private void WelcomeUser()
    {
        Console.WriteLine(DialogOptions.Welcome);
        ChangePage(Page.MainMenu);
    }

    private void UserInputLoop()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? string.Empty;
            
            if (input.ToUpper() == "EXIT")
                Program.Close();
            
            _currentPageHandler.HandleUserInput(input);
        }
    }
}