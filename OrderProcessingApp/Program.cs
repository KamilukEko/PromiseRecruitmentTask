using OrderProcessingApp.Services;

namespace OrderProcessingApp;

public class Program
{
    public static readonly DatabaseManager DatabaseManager = new();
    static void Main(string[] args) => new DialogHandler().Start();
    public static void Close() => Environment.Exit(0);
}