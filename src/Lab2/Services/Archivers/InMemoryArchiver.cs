using Itmo.ObjectOrientedProgramming.Lab2.Models;
using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

public class InMemoryArchiver
{
    private readonly Collection<Message> _messages = [];

    public void Archive(Message message)
    {
        _messages.Add(message);
    }

    public IEnumerable<Message> GetArchivedMessages()
    {
        return _messages.AsReadOnly();
    }

    public void Clear()
    {
        _messages.Clear();
    }

    public int Count => _messages.Count;
}