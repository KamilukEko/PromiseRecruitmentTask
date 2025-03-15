namespace OrderProcessingApp.Tests;

public static class Utils
{
    private static readonly Random Random = new();
    
    public static T GenerateRandomEnum<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Next(values.Length))!;
    }

    public static decimal GenerateRandomDecimal(decimal min = 0.01m, decimal max = 1000m)
    {
        return Math.Round((decimal)(Random.NextDouble() * (double)(max - min) + (double)min), 2);
    }

    public static string GenerateRandomString(int length = 10)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)])
            .ToArray());
    }
}