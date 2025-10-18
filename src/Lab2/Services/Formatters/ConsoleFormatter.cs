using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Formatters;

public class ConsoleFormatter : IMessageFormatter
{
    public void Format(Message message)
    {
        Console.WriteLine("--- Message ---");
        Console.WriteLine($"**Header**: {message.Header}");
        Console.WriteLine($"**Body**: {message.Body}");
        Console.WriteLine($"**Importance**: {message.ImportanceLevel.Name}");
        Console.WriteLine("--- End Message ---");
    }
}