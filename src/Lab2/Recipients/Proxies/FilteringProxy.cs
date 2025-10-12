using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients.Proxies;

public class FilteringProxy : IRecipient
{
    private readonly IRecipient _realRecipient;

    private readonly ImportanceLevel _minImportance;

    public FilteringProxy(IRecipient realRecipient, ImportanceLevel minImportance)
    {
        _realRecipient = realRecipient;
        _minImportance = minImportance;
    }

    public void Receive(Message message)
    {
        if (message.ImportanceLevel.IsAtLeast(_minImportance))
        {
            _realRecipient.Receive(message);
        }
    }
}