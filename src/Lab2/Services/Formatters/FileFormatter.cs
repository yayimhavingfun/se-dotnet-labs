using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Formatters;

public class FileFormatter : IMessageFormatter
{
    private readonly string _filePath;

    public FileFormatter(string filePath)
    {
        _filePath = filePath;
    }

    public void Format(Message message)
    {
        string markdownContent =
            $"# {message.Header}\n\n**Importance level**: {message.ImportanceLevel.Name}\n\n{message.Body}\n\n---\n";
        File.AppendAllText(_filePath, markdownContent);
    }
}