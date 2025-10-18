using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

namespace Itmo.ObjectOrientedProgramming.Lab2.Models;

public class Topic
{
    public string Name { get; }

    private readonly HashSet<IRecipient> _recipients;

    public Topic(string name)
    {
        Name = name;
        _recipients = [];
    }

    public Topic(string name, params IRecipient[] recipients) : this(name)
    {
        foreach (IRecipient recipient in recipients)
        {
            _recipients.Add(recipient);
        }
    }

    public void Send(Message message)
    {
        foreach (IRecipient recipient in _recipients)
        {
            recipient.Receive(message);
        }
    }

    public void AddRecipient(IRecipient recipient)
    {
        _recipients.Add(recipient);
    }

    public void RemoveRecipient(IRecipient recipient)
    {
        _recipients.Remove(recipient);
    }

    public bool ContainsRecipient(IRecipient recipient)
    {
        return _recipients.Contains(recipient);
    }

    public void AddUser(User user)
    {
        _recipients.Add(new UserRecipient(user));
    }

    public void AddArchiver(IArchiver archiver)
    {
        _recipients.Add(new ArchiverRecipient(archiver));
    }

    public void AddAlert(IAlertSystem alert, string[] suspiciousWords)
    {
        _recipients.Add(new AlertRecipient(alert, suspiciousWords));
    }

    public int RecipientCount => _recipients.Count;
}