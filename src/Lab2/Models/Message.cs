namespace Itmo.ObjectOrientedProgramming.Lab2.Models;

public class Message
{
    public Message(string header, string body, ImportanceLevel importanceLevel)
    {
        Header = header;
        Body = body;
        ImportanceLevel = importanceLevel;
    }

    public string Header { get; }

    public string Body { get; }

    public ImportanceLevel ImportanceLevel { get; }

    public override bool Equals(object? obj)
    {
        if (obj is Message other)
        {
            return Header == other.Header &&
                   Body == other.Body &&
                   ImportanceLevel == other.ImportanceLevel;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Header, Body, ImportanceLevel);
    }
}