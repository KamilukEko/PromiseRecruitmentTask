using OrderProcessingApp.Dialogs;
using OrderProcessingApp.Interfaces;
using OrderProcessingApp.Models;
using OrderProcessingApp.Models.Enums;

namespace OrderProcessingApp.Services.PageHandlers;

public class CreateOrderHandler : IPageHandler
{
    private readonly List<Action> _dialogSteps;
    private Action<Page> _changePageFunction;
    private List<string> _orderDetailsValues = [];

    public CreateOrderHandler(Action<Page> changePageFunction)
    {
        _changePageFunction = changePageFunction;
        _dialogSteps = 
        [
            () => Console.Write(CreateOrderDialog.ProductName),
            () => Console.Write(CreateOrderDialog.CustomerType),
            () => Console.Write(CreateOrderDialog.DeliveryAddress),
            () => Console.Write(CreateOrderDialog.PaymentMethod),
            CreateOrder
        ];
    }
    
    public void DisplayOptions() => Console.Write(CreateOrderDialog.InitialMessage + "\n" + CreateOrderDialog.OrderValue);
    
    private void AddValueAndMoveToNextOne(string value)
    {
        _orderDetailsValues.Add(value);

        _dialogSteps[0]();
        _dialogSteps.RemoveAt(0);
    }

    private bool ValidateOrder(out Order order)
    {
        bool success = true;
        
        if (!decimal.TryParse(_orderDetailsValues[0], out decimal orderAmount))
        {
            Console.WriteLine(CreateOrderDialog.InvalidOrderAmount);
            orderAmount = 0;
            success = false;
        }

        string productName = _orderDetailsValues[1];
        if (productName == String.Empty)
        {
            Console.WriteLine(CreateOrderDialog.InvalidProductName);
            success = false;
        }
        
        if (!Enum.TryParse(_orderDetailsValues[2], true, out ClientType clientType))
        {
            Console.WriteLine(CreateOrderDialog.InvalidClientType);
            clientType = ClientType.Unknown;
            success = false;
        }

        string shippingAddress = _orderDetailsValues[3];
        if (productName == String.Empty)
        {
            Console.WriteLine(CreateOrderDialog.InvalidShippingAddress);
            success = false;
        }
        
        if (!Enum.TryParse(_orderDetailsValues[4], true, out PaymentMethod paymentMethod))
        {
            Console.WriteLine(CreateOrderDialog.InvalidClientType);
            paymentMethod = PaymentMethod.Unknown;
            success = false;
        }

        order = new Order
        {
            TotalAmount = orderAmount,
            ProductName = productName,
            ClientType = clientType,
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            Status = success ? OrderStatus.New : OrderStatus.Error
        };
        
        return success;
    }

    private void CreateOrder()
    {
        if (ValidateOrder(out Order order))
        {
            Program.DatabaseManager.AddOrder(order);
            Console.WriteLine(CreateOrderDialog.OrderCreated);
        }
        else
        {
            Console.WriteLine(CreateOrderDialog.OrderCreationFailed);
        }
            
        Console.Write(CreateOrderDialog.AnotherOrderOrReturn);
    }
    
    public void HandleUserInput(string userInput)
    {
        if (_orderDetailsValues.Count != 5)
        {
            AddValueAndMoveToNextOne(userInput);
            return;
        }
        
        if (userInput.ToLower() == "y")
        {
            _changePageFunction(Page.CreateOrder);
            return;
        }
        
        if (userInput.ToLower() == "n")
        {
            _changePageFunction(Page.MainMenu);
            return;
        }
        
        Console.Write(SharedDialogs.InvalidOption);
    }
}