namespace Itmo.ObjectOrientedProgramming.Lab2.Models;

public class User
{
    public Guid Id { get; }

    public string Name { get; }

    public User(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    private readonly Dictionary<Message, MessageStatus> _messageStatusMap = new();

    public IReadOnlyCollection<Message> ReceivedMessages => _messageStatusMap.Keys.ToList().AsReadOnly();

    public int UnreadCount => _messageStatusMap.Count(ms => !ms.Value.IsRead);

    public void ReceiveMessage(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (!_messageStatusMap.ContainsKey(message))
        {
            _messageStatusMap[message] = new MessageStatus(false, DateTime.Now);
        }
    }

    public void MarkAsRead(Message message)
    {
        if (!_messageStatusMap.TryGetValue(message, out MessageStatus? status))
            throw new ArgumentException("Message not found for this user");
        if (status.IsRead)
            throw new InvalidOperationException($"Message is already marked as read");

        _messageStatusMap[message] = status with { IsRead = true, ReadAt = DateTime.Now };
    }

    public bool IsRead(Message message)
    {
        return _messageStatusMap.ContainsKey(message) && _messageStatusMap[message].IsRead;
    }

    public DateTime GetReceivedTime(Message message)
    {
        if (!_messageStatusMap.TryGetValue(message, out MessageStatus? status))
            throw new ArgumentException("Message not found for this user");

        return _messageStatusMap[message].ReceivedAt;
    }

    public DateTime? GetReadTime(Message message)
    {
        if (!_messageStatusMap.TryGetValue(message, out MessageStatus? status))
            throw new ArgumentException("Message not found for this user");

        return _messageStatusMap[message].ReadAt;
    }

    private record MessageStatus(bool IsRead, DateTime ReceivedAt)
    {
        public DateTime? ReadAt { get; init; }
    }
}