using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Logging;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients.Decorators;

public class LoggingDecorator : IRecipient
{
    private readonly IRecipient _wrapped;

    private readonly ILogger _logger;

    public LoggingDecorator(IRecipient wrapped, ILogger logger)
    {
        _wrapped = wrapped;
        _logger = logger;
    }

    public void Receive(Message message)
    {
        try
        {
            _wrapped.Receive(message);
            _logger.Log($"Message processed successfully: {message.Header}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to process message '{message.Header}': {ex.Message}");
            throw;
        }
    }
}