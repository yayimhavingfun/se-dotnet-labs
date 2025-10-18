using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

namespace Itmo.ObjectOrientedProgramming.Lab2;

public class MessageDistributor
{
    private readonly Dictionary<string, Topic> _topics = new();

    public void CreateTopic(string topicName)
    {
        _topics[topicName] = new Topic(topicName);
    }

    public void CreateTopic(string topicName, params IRecipient[] recipients)
    {
        _topics[topicName] = new Topic(topicName, recipients);
    }

    public void AddToTopic(string topicName, IRecipient recipient)
    {
        if (_topics.TryGetValue(topicName, out Topic? topic))
        {
            topic.AddRecipient(recipient);
        }
        else
        {
            throw new ArgumentException($"Topic '{topicName}' not found");
        }
    }

    public void AddUserToTopic(string topicName, User user)
    {
        if (_topics.TryGetValue(topicName, out Topic? topic))
        {
            topic.AddUser(user);
        }
        else
        {
            throw new ArgumentException($"Topic '{topicName}' not found");
        }
    }

    public void AddArchiverToTopic(string topicName, IArchiver archiver)
    {
        if (_topics.TryGetValue(topicName, out Topic? topic))
        {
            topic.AddArchiver(archiver);
        }
        else
        {
            throw new ArgumentException($"Topic '{topicName}' not found");
        }
    }

    public void AddAlertToTopic(string topicName, IAlertSystem alertSystem, string[] suspiciousWords)
    {
        if (_topics.TryGetValue(topicName, out Topic? topic))
        {
            topic.AddAlert(alertSystem, suspiciousWords);
        }
        else
        {
            throw new ArgumentException($"Topic '{topicName}' not found");
        }
    }

    // sending messages
    public void SendToTopic(string topicName, Message message)
    {
        if (_topics.TryGetValue(topicName, out Topic? topic))
        {
            topic.Send(message);
        }
        else
        {
            throw new ArgumentException($"Topic '{topicName}' not found");
        }
    }

    public void SendToAllTopics(Message message)
    {
        foreach (Topic topic in _topics.Values)
        {
            topic.Send(message);
        }
    }

    public Topic? GetTopic(string topicName)
    {
        return _topics.GetValueOrDefault(topicName);
    }

    public IReadOnlyDictionary<string, Topic> Topics => _topics;
}