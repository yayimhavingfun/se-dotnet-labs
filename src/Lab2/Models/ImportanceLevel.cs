namespace Itmo.ObjectOrientedProgramming.Lab2.Models;

public class ImportanceLevel
{
    private ImportanceLevel(string name, int priority)
    {
        Name = name;
        Priority = priority;
    }

    public static ImportanceLevel Low => new("Low", 1);

    public static ImportanceLevel Normal => new("Normal", 2);

    public static ImportanceLevel High => new("High", 3);

    public bool IsAtLeast(ImportanceLevel other)
    {
        return Priority >= other.Priority;
    }

    public string Name { get; }

    public int Priority { get; }
}