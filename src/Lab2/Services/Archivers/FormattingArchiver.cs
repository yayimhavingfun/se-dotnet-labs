using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Formatters;
using System.Collections.ObjectModel;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

public class FormattingArchiver : IArchiver
{
    private readonly IMessageFormatter _messageFormatter;

    private readonly Collection<Message> _messages = [];

    public FormattingArchiver(IMessageFormatter messageFormatter)
    {
        _messageFormatter = messageFormatter;
    }

    public void Archive(Message message)
    {
        _messages.Add(message);
        _messageFormatter.Format(message);
    }

    public IEnumerable<Message> GetArchivedMessages()
    {
        return _messages.AsReadOnly();
    }
}