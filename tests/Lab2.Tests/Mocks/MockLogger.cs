using Itmo.ObjectOrientedProgramming.Lab2.Services.Logging;
using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests.Mocks;

public class MockLogger : ILogger
{
    public int CallCount { get; private set; }

    public Collection<string> Logs { get; } = [];

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

    public bool ContainsLog(string searchText)
    {
        return Logs.Any(log => log.Contains(searchText, StringComparison.OrdinalIgnoreCase));
    }

    public void DisplayCallCount()
    {
        Logs.Add($"Call count: {CallCount}");
    }
}