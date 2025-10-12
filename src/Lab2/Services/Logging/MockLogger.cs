using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Logging;

public class MockLogger : ILogger
{
    private Collection<string> Logs { get; } = [];

    private int CallCount { get; set; }

    public void Log(string message)
    {
        Logs.Add($"[INFO] {message}");
        CallCount++;
    }

    public void LogError(string err)
    {
        Logs.Add($"[ERR] {err}");
        CallCount++;
    }

    public void LogWarning(string warning)
    {
        Logs.Add($"[WARN] {warning}");
        CallCount++;
    }

    public void DisplayCallCount()
    {
        Logs.Add($"Call count: {CallCount}");
    }
}