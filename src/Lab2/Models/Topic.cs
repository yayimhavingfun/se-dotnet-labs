using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

namespace Itmo.ObjectOrientedProgramming.Lab2.Models;

public class Topic
{
    public string Name { get; }

    private readonly GroupRecipient _recipients;

    public Topic(string name)
    {
        Name = name;
        _recipients = new GroupRecipient();
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
        _recipients.Receive(message);
    }

    public void AddRecipient(IRecipient recipient)
    {
        _recipients.Add(recipient);
    }

    public void RemoveRecipient(IRecipient recipient)
    {
        _recipients.Remove(recipient);
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

    public int RecipientCount => _recipients.RecipientCount();
}