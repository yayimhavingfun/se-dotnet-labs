namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Logging;

public interface ILogger
{
    void Log(string message);

    void LogError(string err);

    void LogWarning(string warning);
}