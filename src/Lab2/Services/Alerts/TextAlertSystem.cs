namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;

public class TextAlertSystem : IAlertSystem
{
    public string Name => "Text Alert System";

    public void Alert(string message)
    {
        Console.WriteLine($"!!! ALERT: {message} !!!");
        Console.WriteLine($"Timestamp: {DateTime.Now:HH:mm:ss}");
        Console.WriteLine(new string('=', 50));
    }
}