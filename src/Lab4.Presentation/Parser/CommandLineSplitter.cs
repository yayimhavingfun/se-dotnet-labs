using System.Text;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;

public class CommandLineSplitter
{
    public IReadOnlyList<string> Split(string commandLine)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
        {
            return Array.Empty<string>();
        }

        var parts = new List<string>();
        var currentPart = new StringBuilder();
        bool inQuotes = false;
        char quoteChar = '\0';

        for (int i = 0; i < commandLine.Length; i++)
        {
            char currentChar = commandLine[i];

            if (IsQuote(currentChar))
            {
                if (!inQuotes)
                {
                    inQuotes = true;
                    quoteChar = currentChar;
                    currentPart.Append(currentChar);
                }
                else if (currentChar == quoteChar)
                {
                    inQuotes = false;
                    currentPart.Append(currentChar);

                    parts.Add(RemoveOuterQuotes(currentPart.ToString()));
                    currentPart.Clear();
                }
                else
                {
                    currentPart.Append(currentChar);
                }
            }
            else if (char.IsWhiteSpace(currentChar) && !inQuotes)
            {
                if (currentPart.Length > 0)
                {
                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                }
            }
            else
            {
                currentPart.Append(currentChar);
            }
        }

        if (currentPart.Length > 0)
        {
            string part = currentPart.ToString();
            if (inQuotes)
            {
                parts.Add(part);
            }
            else
            {
                parts.Add(part);
            }
        }

        return parts.AsReadOnly();
    }

    public string[] SplitToArray(string commandLine)
    {
        return Split(commandLine).ToArray();
    }

    public string Join(IEnumerable<string> parts)
    {
        var result = new StringBuilder();
        bool first = true;

        foreach (string part in parts)
        {
            if (!first)
            {
                result.Append(' ');
            }

            if (part.Contains(' ', StringComparison.Ordinal) || part.Contains('"', StringComparison.Ordinal) || part.Contains('\'', StringComparison.Ordinal))
            {
                string escaped = part.Replace("\"", "\\\"", StringComparison.Ordinal);
                result.Append('"').Append(escaped).Append('"');
            }
            else
            {
                result.Append(part);
            }

            first = false;
        }

        return result.ToString();
    }

    private bool IsQuote(char c)
    {
        return c == '"' || c == '\'';
    }

    private string RemoveOuterQuotes(string str)
    {
        if (str.Length < 2)
            return str;

        char firstChar = str[0];
        char lastChar = str[^1];

        if (IsQuote(firstChar) && firstChar == lastChar)
        {
            return str[1..^1];
        }

        return str;
    }
}