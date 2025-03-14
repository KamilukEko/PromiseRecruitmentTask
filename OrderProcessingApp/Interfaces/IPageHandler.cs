using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Interfaces;

public interface IPageHandler
{
    void DisplayOptions();
    void HandleUserInput(string userInput);
}