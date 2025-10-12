using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class AlertRecipient : IRecipient
{
    private readonly IAlertSystem _alertSystem;
    private readonly string[] _suspiciousWords;
    private readonly bool _checkHeader;
    private readonly bool _checkBody;

    public AlertRecipient(
        IAlertSystem alertSystem,
        string[] suspiciousWords,
        bool checkHeader = true,
        bool checkBody = true)
    {
        _alertSystem = alertSystem;
        _suspiciousWords = suspiciousWords;
        _checkHeader = checkHeader;
        _checkBody = checkBody;
    }

    public void Receive(Message message)
    {
        if (ContainsSuspiciousContent(message))
        {
            _alertSystem.Alert($"Suspicious content detected in message: '{message.Header}'");
        }
    }

    private bool ContainsSuspiciousContent(Message message)
    {
        var textToCheck = new List<string>();

        if (_checkHeader)
            textToCheck.Add(message.Header);

        if (_checkBody)
            textToCheck.Add(message.Body);

        return textToCheck.Any(text =>
            _suspiciousWords.Any(word =>
                text.Contains(word, StringComparison.OrdinalIgnoreCase)));
    }
}