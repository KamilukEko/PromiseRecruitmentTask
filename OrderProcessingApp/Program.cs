using OrderProcessingApp.Services;

namespace OrderProcessingApp;

class Program
{
    static void Main(string[] args) => new DialogHandler().Start();
    public static void Close() => Environment.Exit(0);
}