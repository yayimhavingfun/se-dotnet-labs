namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;

public interface IAlertSystem
{
    void Alert(string message);

    string Name { get; }
}