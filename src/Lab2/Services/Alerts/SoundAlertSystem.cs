namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;

public class SoundAlertSystem : IAlertSystem
{
    public string Name => "Sound Alert System";

    public void Alert(string message)
    {
        Console.Beep();
    }
}