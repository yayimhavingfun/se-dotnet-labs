using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class GroupRecipient
{
    private readonly List<IRecipient> _recipients = [];

    public void Add(IRecipient recipient)
    {
        _recipients.Add(recipient);
    }

    public void Remove(IRecipient recipient)
    {
        _recipients.Remove(recipient);
    }

    public void Receive(Message message)
    {
        foreach (IRecipient recipient in _recipients)
        {
            recipient.Receive(message);
        }
    }

    public int RecipientCount()
    {
        return _recipients.Count;
    }
}